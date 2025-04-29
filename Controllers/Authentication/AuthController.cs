using System.Security.Claims;
using Firebase_Auth.Data.Constant;
using Firebase_Auth.Data.Models.Authentication.DTO;
using Firebase_Auth.Data.Models.Authentication.DTO.social;
using Firebase_Auth.Services.Authentication.Interfaces;
using Firebase_Auth.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firebase_Auth.Controllers.Authentication;
[Route("api/auth")]
public class AuthController : CoreController
{
    private readonly IAuthService _authService;
    private readonly ICookieManage _cookieManager;
    public AuthController(IAuthService authService, ICookieManage cookieManager, ILogger<AuthController> logger) : base(logger)
    {
        _authService = authService;
        _cookieManager = cookieManager;
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterWithEmailAndPasswordAsync([FromBody] RegisterRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return ToBadRequest(ModelState);
            }

            var result = await _authService.RegisterWithEmailAndPasswordAsync(request);
            return ToSuccess("Registration successful!", result);
        }
        catch (ApplicationException ex) when (ex.Message.Contains("already exists"))
        {
            return ToConflict("This email is already exists!");
        }
        catch (ApplicationException ex)
        {
            return ToBadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user registration");
            return ToInternalServerError($"An unexpected error occurred during registration. Please try again later. :  {ex.Message}");
        }
    }
    [HttpPost("login")]
    public async Task<IActionResult> LoginWithEmailAndPassword([FromBody] LoginRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return ToBadRequest(ModelState);
            }
            var clientType = Request.Headers["X-Client-Type"].ToString();
            var result = await _authService.LoginWithEmailAndPasswordAsync(request);

            //if client is web is good to set refresh token in the cookie instead of res as body
            if (clientType == "web")
            {
                _cookieManager.SetRefreshTokenCookie(Response, result.RefreshToken);
                return ToSuccess("Login success.", result.AccessToken);
            }

            //if client != web then we can res with refreshToken, Ex: Mobile ...
            return ToSuccess("Login success.", result);
        }
        catch (KeyNotFoundException ex)
        {
            return ToNotFound(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return ToUnauthorized(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during user login");
            return ToInternalServerError($"An unexpected error occurred during login. Please try again later. :  {ex.Message}");
        }
    }

    [Authorize]
    [HttpPost("logout")]
    public async Task<IActionResult> LogoutAsync()
    {
        try
        {
            //var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var userId = User.FindFirst("user_id")?.Value ?? User.FindFirst("sub")?.Value;
            await _authService.LogoutAsync(userId);
            var clientType = Request.Headers["X-Client-Type"].ToString();
            if (clientType == "web")
            {
                _cookieManager.DeleteRefreshTokenCookie(Response);
            }
            return ToNoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return ToNotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return ToInternalServerError(ex.Message);
        }
    }
    [Authorize(Roles = RoleNames.Root)]
    [HttpPost("get-user")]
    public async Task<IActionResult> GetUserInfoByTokenId(string idToken)
    {
        try
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var result = await _authService.VerifyAndGetUserAsync(idToken);
            return ToSuccess("Success", result);
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync(string refreshToken)
    {
        var result = await _authService.RefreshTokenAsync(refreshToken);
        return ToSuccess("Success", result);
    }

    [HttpPost("social-signin")]
    public async Task<IActionResult> SocialSignIn([FromBody] SocialSignInRequest request)
    {
        try
        {
            var result = await _authService.SignWithSocialProvideAsync(request);
            return Ok(result);
        }
        catch (ApplicationException ex)
        {
            return ToBadRequest(ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            return ToUnauthorized(ex.Message);
        }
        catch (Exception)
        {
            return ToInternalServerError("An error occurred during authentication.");
        }
    }

    [Authorize]
    [HttpGet("test")]
    public async Task<IActionResult> Test()
    {
        try
        {
            var role = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            await Task.Delay(1000);
            return ToSuccess(role ?? "Role is null");
        }
        catch (Exception)
        {
           return ToInternalServerError("An error occurred during authentication.");
        }
    }
}