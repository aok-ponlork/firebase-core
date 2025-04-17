namespace Firebase_Auth.Data.Models.Authentication.DTO.social;
public class SocialSignInRequest
{
    public string ProviderToken { get; set; } = string.Empty;
    public string ProviderId { get; set; } = string.Empty;  // "facebook.com" or "google.com"
    public string RequestUri { get; set; } = string.Empty;
}