using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AutoMapper;
using Firebase_Auth.Context;
using Firebase_Auth.Data.Constant;
using Firebase_Auth.Data.Entities.Authentication;
using Firebase_Auth.Data.Models.Authentication;
using Firebase_Auth.Data.Models.Authentication.DTO;
using Firebase_Auth.Data.Models.Authentication.DTO.social;
using Firebase_Auth.Helper.Firebase;
using Firebase_Auth.Helper.Firebase.social;
using Firebase_Auth.Services.Authentication.Interfaces;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;

namespace Firebase_Auth.Services.Authentication;

internal sealed class AuthService(FirebaseAuth firebaseAuth, CoreDbContext context, IMapper mapper, IConfiguration configuration, IHttpClientFactory httpClientFactory) : IAuthService
{
    private readonly FirebaseAuth _firebaseAuth = firebaseAuth;
    private readonly CoreDbContext _context = context;
    private readonly IMapper _mapper = mapper;
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient("httpClient");
    private readonly IConfiguration _configuration = configuration;

    public async Task<TokenModel> LoginWithEmailAndPasswordAsync(LoginRequest request)
    {
        // 1. Get user from database
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email)
            ?? throw new KeyNotFoundException($"No account found associated with the email '{request.Email}'. Please check the email address or register if you don't have an account.");

        // 2. Authenticate with Firebase
        var authResult = await AuthenticateWithFirebaseAsync(request.Email, request.Password);

        // 3. Verify the Firebase UID matches our database record
        if (user.FirebaseUid != authResult.LocalId)
        {
            throw new UnauthorizedAccessException("Account authentication mismatch.");
        }
        // 4. Return the tokenModel
        return new TokenModel(
            accessToken: authResult.IdToken,
            refresToken: authResult.RefreshToken,
            expiresIn: authResult.ExpiresIn,
            uId: authResult.LocalId
        );
    }

    public async Task<TokenModel> RefreshTokenAsync(string refreshToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new UnauthorizedAccessException("Refresh token is missing.");
        }
        var authUrl = _configuration["Firebase:RefreshTokenUri"];

        var content = new StringContent(
            JsonSerializer.Serialize(new { refresh_token = refreshToken, grant_type = "refresh_token" }),
            Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(authUrl, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        // Check if the response is successful
        if (!response.IsSuccessStatusCode)
        {
            // Check for specific Firebase error
            if (responseContent.Contains("INVALID_REFRESH_TOKEN"))
            {
                throw new UnauthorizedAccessException("The provided refresh token is invalid.");
            }

            // General error handling
            throw new ApplicationException($"Failed to refresh token: {responseContent}");
        }

        // Deserialize response into FirebaseAuthResponse
        var authResult = JsonSerializer.Deserialize<FirebaseRefreshTokenResponse>(responseContent) ?? throw new ApplicationException("Failed to parse Firebase authentication response.");
        // Return the new access token and refresh token
        return new TokenModel(authResult.IdToken, authResult.RefreshToken, authResult.ExpiresIn ?? "", authResult.LocalId ?? "");
    }

    public async Task<UserModel> RegisterWithEmailAndPasswordAsync(RegisterRequest request)
    {
        try
        {
            // 1. Check if email already exists in database
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new ApplicationException("Email already exists in the system.");
            }

            // 2. Save user information in Firebase
            var user = new UserRecordArgs
            {
                Email = request.Email,
                Password = request.Password,
                DisplayName = request.UserName,
                EmailVerified = false,
            };

            UserRecord firebaseUser;
            try
            {
                firebaseUser = await _firebaseAuth.CreateUserAsync(user);
            }
            catch (FirebaseAuthException ex) when (ex.AuthErrorCode == AuthErrorCode.EmailAlreadyExists)
            {
                throw new ApplicationException("Email already registered with authentication provider.", ex);
            }
            catch (FirebaseAuthException ex)
            {
                throw new ApplicationException($"Authentication error: {ex.Message}", ex);
            }

            var firebaseUid = firebaseUser.Uid;

            // 3. Save user into database
            var createUser = new User
            {
                Email = request.Email,
                UserName = request.UserName,
                FirebaseUid = firebaseUid,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                State = EfState.Active,
                RoleId = Guid.Parse("11111111-1111-1111-1111-111111111111"), // sign as User role
            };

            await _context.Users.AddAsync(createUser);
            await _context.SaveChangesAsync();
            var claims = new Dictionary<string, object>
            {
                { "role", RoleNames.User }
            };
            await _firebaseAuth.SetCustomUserClaimsAsync(firebaseUid, claims);
            var response = _mapper.Map<UserModel>(createUser);
            return response;
        }
        catch (DbUpdateException ex)
        {
            throw new ApplicationException("Database error occurred while registering user.", ex);
        }
        catch (Exception ex) when (!(ex is ApplicationException))
        {
            throw new ApplicationException("An unexpected error occurred during registration.", ex);
        }
    }

    public async Task<UserModel> VerifyAndGetUserAsync(string idToken)
    {
        try
        {
            // 1. Verify the Firebase token
            var decodedToken = await _firebaseAuth.VerifyIdTokenAsync(idToken);
            var firebaseUid = decodedToken.Uid;

            // 2. Get user from database
            var user = await _context.Users.FirstOrDefaultAsync(u => u.FirebaseUid == firebaseUid);

            // 3. Handle case where user exists in Firebase but not in our database
            if (user == null)
            {
                // Get user info from Firebase
                var firebaseUser = await _firebaseAuth.GetUserAsync(firebaseUid);
                await _firebaseAuth.RevokeRefreshTokensAsync(firebaseUid);
                // Create user in our database
                user = new User
                {
                    FirebaseUid = firebaseUid,
                    Email = firebaseUser.Email,
                    UserName = firebaseUser.DisplayName ?? firebaseUser.Email?.Split('@')[0] ?? firebaseUid,
                    FullName = firebaseUser.DisplayName,
                    PhotoUrl = firebaseUser.PhotoUrl,
                    CreatedOn = DateTime.UtcNow,
                    IsActive = true,
                };

                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
            }

            // 4. Update last login timestamp
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return _mapper.Map<UserModel>(user);
        }
        catch (FirebaseAuthException)
        {
            throw new UnauthorizedAccessException();
        }
        catch (Exception)
        {
            throw new Exception();
        }

    }

    //Social sign in 
    public async Task<TokenModel> SignWithSocialProvideAsync(SocialSignInRequest request)
    {
        // 1. Validate the token with Firebase
        var firebaseAuthResult = await ExchangeProviderTokenWithFirebaseAsync(
            request.ProviderToken,
            request.ProviderId, // "facebook.com" or "google.com"
            request.RequestUri);

        // 2. Check if user exists in your database
        var existingUser = await _context.Users
            .FirstOrDefaultAsync(u => u.FirebaseUid == firebaseAuthResult.LocalId);

        if (existingUser == null)
        {
            // 3. If user doesn't exist, create a new one
            var userProfile = await GetUserProfileFromProviderAsync(
                request.ProviderToken,
                request.ProviderId
            );

            var newUser = new User
            {
                FirebaseUid = firebaseAuthResult.LocalId,
                Email = firebaseAuthResult.Email,
                FullName = userProfile.FullName,
                PhotoUrl = userProfile.PhotoUrl,
                CreatedOn = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            existingUser = newUser;
        }
        else
        {
            existingUser.LastLoginAt = DateTime.UtcNow;
            _context.Users.Update(existingUser);
            await _context.SaveChangesAsync();
        }

        // 5. Return authentication tokens
        return new TokenModel(
            accessToken: firebaseAuthResult.IdToken,
            refresToken: firebaseAuthResult.RefreshToken,
            expiresIn: firebaseAuthResult.ExpiresIn,
            uId: existingUser.FirebaseUid
        );
    }

    public async Task LogoutAsync(string? firebaseUid)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.FirebaseUid == firebaseUid)
       ?? throw new KeyNotFoundException($"User with ID '{firebaseUid}' not found.");
        //  Revoke Firebase token
        await _firebaseAuth.RevokeRefreshTokensAsync(firebaseUid);
        // Update recode time zone should add + 7 
        user.UpdatedOn = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    private async Task SetCustomClaimsAsync(string uid, string role, List<string> permissions)
    {
        var claims = new Dictionary<string, object>
        {
            { "role", role },
            { "permissions", permissions }
        };

        await _firebaseAuth.SetCustomUserClaimsAsync(uid, claims);
    }
    #region Helper
    private async Task<FirebaseAuthResponse> AuthenticateWithFirebaseAsync(string email, string password)
    {
        var authUrl = _configuration["Firebase:signInWithPassword"];

        using var content = new StringContent(
            JsonSerializer.Serialize(
            new
            {
                email,
                password,
                returnSecureToken = true
            }),
                Encoding.UTF8, "application/json"
            );

        var response = await _httpClient.PostAsync(authUrl, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            if (responseContent.Contains("INVALID_LOGIN_CREDENTIALS"))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }
            if (responseContent.Contains("USER_DISABLED"))
            {
                throw new UnauthorizedAccessException("Access denied. This account has been disabled.");
            }
            throw new ApplicationException($"Firebase authentication failed: {responseContent}");
        }

        var authResult = JsonSerializer.Deserialize<FirebaseAuthResponse>(responseContent);
        return authResult ?? throw new ApplicationException("Failed to parse authentication response");
    }


    private async Task<FirebaseAuthResponse> ExchangeProviderTokenWithFirebaseAsync(string providerToken, string providerId, string requestUri)
    {
        var authUrl = _configuration["Firebase:signInWithThirdPartyProvider"];

        var requestData = new
        {
            postBody = $"access_token={providerToken}&providerId={providerId}",
            requestUri,
            returnIdpCredential = true,
            returnSecureToken = true
        };

        using var content = new StringContent(
            JsonSerializer.Serialize(requestData),
            Encoding.UTF8,
            "application/json");

        var response = await _httpClient.PostAsync(authUrl, content);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            if (responseContent.Contains("INVALID_ID_TOKEN"))
            {
                throw new UnauthorizedAccessException("Invalid provider token.");
            }

            throw new ApplicationException($"Firebase authentication failed: {responseContent}");
        }
        var authResult = JsonSerializer.Deserialize<FirebaseAuthResponse>(responseContent);
        return authResult ?? throw new ApplicationException("Failed to parse authentication response");
    }



    // Helper method to get additional user info from social provider if needed
    private async Task<UserModel> GetUserProfileFromProviderAsync(string providerToken, string providerId)
    {
        if (providerId == "google.com")
        {
            return await GetGoogleUserProfileAsync(providerToken);

        }
        // else if (providerId == "facebook.com")
        // {
        //     return await GetFacebookUserProfileAsync(providerToken);
        // }
        throw new NotSupportedException($"Provider {providerId} is not supported.");
    }
    private async Task<UserModel> GetGoogleUserProfileAsync(string accessToken)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

        var response = await _httpClient.GetAsync("https://www.googleapis.com/oauth2/v2/userinfo");
        var content = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new ApplicationException($"Failed to get Google profile: {content}");
        }

        var googleProfile = JsonSerializer.Deserialize<GoogleProfile>(content);

        return new UserModel
        {
            FullName = googleProfile?.Name ?? "",
            Email = googleProfile?.Email ?? "",
            PhotoUrl = googleProfile?.Picture ?? ""
        };
    }
    //Get user in from Facebook
    // private async Task<UserProfile> GetFacebookUserProfileAsync(string accessToken)
    // {
    //     var httpClient = _httpClientFactory.CreateClient();
    //     var response = await httpClient.GetAsync(
    //         $"https://graph.facebook.com/me?fields=id,name,email,picture&access_token={accessToken}");

    //     var content = await response.Content.ReadAsStringAsync();
    //     if (!response.IsSuccessStatusCode)
    //     {
    //         throw new ApplicationException($"Failed to get Facebook profile: {content}");
    //     }

    //     var fbProfile = JsonSerializer.Deserialize<FacebookProfile>(content);

    //     return new UserProfile
    //     {
    //         DisplayName = fbProfile?.Name ?? "",
    //         Email = fbProfile?.Email ?? "",
    //         PhotoUrl = fbProfile?.Picture?.Data?.Url ?? ""
    //     };
    // }
    #endregion
    public Task SendEmailVerification()
    {
        throw new NotImplementedException();
    }
}