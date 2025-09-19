namespace Firebase_Auth.Infrastructure.Persistence.Redis.Interfaces;

public interface IRedisRepository<T> where T : class
{
    // Basic CRUD Operations
    Task<T?> GetAsync(string key);
    Task<bool> SetAsync(string key, T value, TimeSpan? expiration = null);
    Task<bool> DeleteAsync(string key);
    Task<bool> ExistsAsync(string key);

    // Bulk Operations
    Task<Dictionary<string, T?>> GetManyAsync(IEnumerable<string> keys);
    Task<bool> SetManyAsync(Dictionary<string, T> keyValuePairs, TimeSpan? expiration = null);
    Task<long> DeleteManyAsync(IEnumerable<string> keys);

    // List Operations
    Task<IEnumerable<T>> GetListAsync(string key);
    Task<bool> SetListAsync(string key, IEnumerable<T> values, TimeSpan? expiration = null);
    Task<long> AddToListAsync(string key, T value);
    Task<long> AddToListAsync(string key, IEnumerable<T> values);

    // Hash Operations
    Task<bool> SetHashAsync(string key, string field, T value);
    Task<T?> GetHashAsync(string key, string field);
    Task<Dictionary<string, T?>> GetHashAllAsync(string key);
    Task<bool> DeleteHashFieldAsync(string key, string field);

    // Search and Pattern Operations
    Task<IEnumerable<string>> GetKeysAsync(string pattern = "*");
    Task<long> GetCountAsync(string pattern = "*");

    // Expiration Management
    Task<bool> SetExpirationAsync(string key, TimeSpan expiration);
    Task<TimeSpan?> GetTimeToLiveAsync(string key);

    // Cache Management
    Task FlushDatabaseAsync();
    Task<bool> PingAsync();
}