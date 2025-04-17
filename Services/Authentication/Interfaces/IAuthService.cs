using Firebase_Auth.Data.Models.Authentication;
using Firebase_Auth.Data.Models.Authentication.DTO;
using Firebase_Auth.Data.Models.Authentication.DTO.social;

namespace Firebase_Auth.Services.Authentication.Interfaces;
public interface IAuthService
{
    Task<string> RegisterWithEmailAndPasswordAsync(RegisterRequest request);
    Task<TokenModel> LoginWithEmailAndPasswordAsync(LoginRequest request);
    Task LogoutAsync(string? firebaseUid);
    Task<TokenModel> SignWithSocialProvideAsync(SocialSignInRequest request);
    Task<TokenModel> RefreshTokenAsync(string refreshToken);
    Task<UserModel> VerifyAndGetUserAsync(string idToken);
}