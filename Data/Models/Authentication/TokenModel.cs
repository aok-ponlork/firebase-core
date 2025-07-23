namespace Firebase_Auth.Data.Models.Authentication;
public class TokenModel(string accessToken, string refresToken, string expiresIn, string uId)
{
    public string AccessToken { get; set; } = accessToken;
    public string RefreshToken { get; set; } = refresToken;
    public string ExpiresIn { get; set; } = expiresIn;
    public string? Uid { get; set; } = uId;
}