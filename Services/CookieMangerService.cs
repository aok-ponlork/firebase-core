using Firebase_Auth.Services.Interfaces;
internal sealed class CookieManagerService : ICookieManage
{
    public void SetRefreshTokenCookie(HttpResponse response, string refreshToken)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTime.UtcNow.AddDays(7)
        };
        response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
    }

    public void DeleteRefreshTokenCookie(HttpResponse response)
    {
        response.Cookies.Delete("refreshToken");
    }
}
