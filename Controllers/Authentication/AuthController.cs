using Firebase_Auth.Data.Models.Authentication.DTO;
using Firebase_Auth.Services.Authentication.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Firebase_Auth.Controllers.Authentication;
[Route("api/auth")]
public class AuthController : CoreController
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService, ILogger<AuthController> logger) : base(logger)
    {
        _authService = authService;
    }
    [HttpPost("register")]
    public async Task<IActionResult> RegisterWithEmailAndPasswordAsync([FromBody] RegisterRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
                return BadRequest(ModelState);
            }
            var clientType = Request.Headers["X-Client-Type"].ToString();
            var result = await _authService.LoginWithEmailAndPasswordAsync(request);

            //if client is web is good to set refresh token in the cookie instead of res as body
            if (clientType == "web")
            {
                SetRefreshTokenCookie(result.RefreshToken);
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

    [HttpPost("get-user")]
    public async Task<IActionResult> GetUserInfoByTokenId(string idToken)
    {
        var result = await _authService.VerifyAndGetUserAsync(idToken);
        return ToSuccess("Success", result);
    }


    [HttpPost("refresh")]
    public async Task<IActionResult> RefreshTokenAsync(string refreshToken)
    {
        var result = await _authService.RefreshTokenAsync(refreshToken);
        return ToSuccess("Success", result);
    }

    #region Cookie Helper
    private void SetRefreshTokenCookie(string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }
    #endregion

}