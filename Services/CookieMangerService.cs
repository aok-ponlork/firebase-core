using Firebase_Auth.Services.Interfaces;
internal sealed class CookieManagerService : ICookieManage
{
    public void SetRefreshTokenCookie(HttpResponse response, string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true, // Prevents JS from accessing the cookie (helps against XSS)
            Secure = false,  // Set to true in production (only send over HTTPS)
            SameSite = SameSiteMode.Strict, // CSRF protection: only send cookie in first-party context
            Expires = DateTime.UtcNow.AddDays(7) // Cookie will expire in 7 days
        };
        response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }


    public void DeleteRefreshTokenCookie(HttpResponse response)
    {
        response.Cookies.Delete("refreshToken");
    }
}
