using Firebase_Auth.Data.Models.Authentication;
using Firebase_Auth.Data.Models.Authentication.DTO;

namespace Firebase_Auth.Services.Authentication.Interfaces;
public interface IAuthService
{
    Task<string> RegisterWithEmailAndPasswordAsync(RegisterRequest request);
    Task<TokenModel> LoginWithEmailAndPasswordAsync(LoginRequest request);
    Task<TokenModel> RefreshTokenAsync(string refreshToken);
    Task<UserModel> VerifyAndGetUserAsync(string idToken);
}