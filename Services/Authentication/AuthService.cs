using System.Text;
using System.Text.Json;
using AutoMapper;
using Firebase_Auth.Context;
using Firebase_Auth.Data.Entities.Authentication;
using Firebase_Auth.Data.Models.Authentication;
using Firebase_Auth.Data.Models.Authentication.DTO;
using Firebase_Auth.Helper.Firebase;
using Firebase_Auth.Services.Authentication.Interfaces;
using FirebaseAdmin.Auth;
using Microsoft.EntityFrameworkCore;

namespace Firebase_Auth.Services.Authentication;
internal sealed class AuthService(FirebaseAuth firebaseAuth, CoreDbContext context, IMapper mapper, IConfiguration configuration) : IAuthService
{
    private readonly FirebaseAuth _firebaseAuth = firebaseAuth;
    private readonly CoreDbContext _context = context;
    private readonly IMapper _mapper = mapper;

    private readonly IConfiguration _configuration = configuration;

    public async Task<TokenModel> LoginWithEmailAndPasswordAsync(LoginRequest request)
    {
        try
        {
            //1.Get FirebaseID from User
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email) ?? throw new KeyNotFoundException($"No account found associated with the email '{request.Email}'. Please check the email address or register if you don't have an account.");

            // 2. Use Firebase Auth REST API to sign in with email/password
            var authUrl = _configuration["Firebase:AuthUri"];
            using var httpClient = new HttpClient();
            var content = new StringContent(
                JsonSerializer.Serialize(new
                {
                    email = request.Email,
                    password = request.Password,
                    returnSecureToken = true
                }),
                Encoding.UTF8, "application/json");

            var response = await httpClient.PostAsync(authUrl, content);
            var responseContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {

                if (responseContent.Contains("INVALID_LOGIN_CREDENTIALS"))
                {
                    throw new UnauthorizedAccessException("Invalid email or password.");
                }
                var errorContent = await response.Content.ReadAsStringAsync();
                throw new ApplicationException($"Firebase authentication failed: {errorContent}");
            }
            var authResult = JsonSerializer.Deserialize<FirebaseAuthResponse>(responseContent);

            if (authResult == null)
            {
                throw new ApplicationException("Failed to parse authentication response");
            }

            return new TokenModel(authResult.IdToken, authResult.RefreshToken);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<TokenModel> RefreshTokenAsync(string refreshToken)
    {
        var authUrl = _configuration["Firebase:RefreshTokenUri"];
        using var httpClient = new HttpClient();

        var content = new StringContent(
            JsonSerializer.Serialize(new { refresh_token = refreshToken, grant_type = "refresh_token" }),
            Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync(authUrl, content);
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
        return new TokenModel(authResult.IdToken, authResult.RefreshToken);
    }


    public async Task<string> RegisterWithEmailAndPasswordAsync(RegisterRequest request)
    {
        try
        {
            // 1. Check if email already exists in your database
            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
            {
                throw new ApplicationException("Email already exists in the system.");
            }

            // 2. Save user information in Firebase
            var user = new UserRecordArgs
            {
                Email = request.Email,
                Password = request.Password,
                DisplayName = request.UserName
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
                // Handle other Firebase-specific errors
                throw new ApplicationException($"Authentication error: {ex.Message}", ex);
            }

            var firebaseUid = firebaseUser.Uid;

            // 3. Save user into your database
            var createUser = new User
            {
                Email = request.Email,
                UserName = request.UserName,
                FirebaseUid = firebaseUid,
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
            };

            await _context.Users.AddAsync(createUser);
            await _context.SaveChangesAsync();

            return firebaseUid;
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
        catch (FirebaseAuthException ex)
        {
            throw new UnauthorizedAccessException($"Invalid token: {ex.Message}");
        }
    }
}