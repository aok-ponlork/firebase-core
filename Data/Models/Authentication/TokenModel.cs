namespace Firebase_Auth.Data.Models.Authentication;
public class TokenModel(string accessToken, string refreshtoken)
{
    public string AccessToken { get; set; } = accessToken;
    public string RefreshToken { get; set; } = refreshtoken;
}