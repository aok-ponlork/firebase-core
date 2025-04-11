using System.Text.Json.Serialization;
namespace Firebase_Auth.Helper.Firebase;
public class FirebaseAuthResponse
{
    [JsonPropertyName("idToken")]
    public required string IdToken { get; set; } = "";

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("refreshToken")]
    public required string RefreshToken { get; set; } = "";

    [JsonPropertyName("expiresIn")]
    public string? ExpiresIn { get; set; }

    [JsonPropertyName("localId")]
    public string? LocalId { get; set; }
}

public class FirebaseRefreshTokenResponse
{
    [JsonPropertyName("id_token")]
    public required string IdToken { get; set; } = "";

    [JsonPropertyName("email")]
    public string? Email { get; set; }

    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; set; } = "";

    [JsonPropertyName("expires_in")]
    public string? ExpiresIn { get; set; }

    [JsonPropertyName("local_id")]
    public string? LocalId { get; set; }
}