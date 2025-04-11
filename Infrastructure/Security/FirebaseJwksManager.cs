using Microsoft.IdentityModel.Tokens;

namespace Firebase_Auth.Infrastructure.Security;

public class FirebaseJwksManager
{
    private static readonly SemaphoreSlim SemaphoreSlim = new(1, 1);
    private IEnumerable<SecurityKey>? _cachedSigningKeys;
    private DateTime _keysLastRefreshed = DateTime.MinValue;
    private readonly TimeSpan _refreshInterval = TimeSpan.FromHours(24); // Keys rotate every 24 hours
    private readonly string _jwksUrl;
    private readonly HttpClient _httpClient;

    public FirebaseJwksManager(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _jwksUrl = configuration["Firebase:JwksUrl"] ??
            "https://www.googleapis.com/service_accounts/v1/jwk/securetoken@system.gserviceaccount.com";
        _httpClient = httpClientFactory.CreateClient("httpClient");
    }

    public async Task<IEnumerable<SecurityKey>> GetSigningKeysAsync()
    {
        // Return cached keys if they're still fresh
        if (_cachedSigningKeys != null && DateTime.UtcNow - _keysLastRefreshed < _refreshInterval)
        {
            return _cachedSigningKeys;
        }

        // Otherwise refresh the keys
        await SemaphoreSlim.WaitAsync();
        try
        {
            // Double-check after acquiring the lock
            if (_cachedSigningKeys != null && DateTime.UtcNow - _keysLastRefreshed < _refreshInterval)
            {
                return _cachedSigningKeys;
            }

            var response = await _httpClient.GetStringAsync(_jwksUrl);
            var keys = new JsonWebKeySet(response).GetSigningKeys();

            _cachedSigningKeys = keys;
            _keysLastRefreshed = DateTime.UtcNow;

            return keys;
        }
        catch (Exception ex)
        {
            // Show error 
            Console.WriteLine($"Error refreshing Firebase JWKS: {ex.Message}");

            // If we have cached keys, return them even if they're expired
            if (_cachedSigningKeys != null)
            {
                return _cachedSigningKeys;
            }

            // Otherwise, we have to fail
            throw new InvalidOperationException("Unable to retrieve Firebase signing keys and no cached keys available", ex);
        }
        finally
        {
            SemaphoreSlim.Release();
        }
    }
}