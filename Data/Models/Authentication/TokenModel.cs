namespace Firebase_Auth.Data.Models.Authentication;
public class TokenModel(string accessToken, string refreshtoken, string expiresIn)
{
    public string AccessToken { get; set; } = accessToken;
    public string RefreshToken { get; set; } = refreshtoken;
    public string expiresIn { get; set; } = expiresIn;
}