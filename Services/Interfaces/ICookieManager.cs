namespace Firebase_Auth.Services.Interfaces;
public interface ICookieManage
{
    void SetRefreshTokenCookie(HttpResponse response, string refreshToken);
    void DeleteRefreshTokenCookie(HttpResponse response);
}
