using System.Security.Claims;
using System.Text.Json;
using StackExchange.Redis;

namespace Financist.WebClient.Backend.Session;

public class UserSessionStore : IUserSessionStore
{
    private const int DefaultExpiration = 28800;
    
    private readonly IDatabase _database;
    private readonly TimeSpan _expiration;

    public UserSessionStore(IDatabase database, IConfiguration configuration)
    {
        _database = database;
        _expiration = TimeSpan.FromSeconds(configuration.GetValue("Sessions:Expiration", DefaultExpiration));
    }

    public async Task SetSessionDataAsync(string sessionId, UserSession session)
    {
        var data = JsonSerializer.Serialize(session);
        await _database.StringSetAsync(sessionId, data, _expiration);
    }

    public async Task<UserSession?> GetSessionDataAsync(string sessionId)
    {
        var data = await _database.StringGetAsync(sessionId);
        if (!data.HasValue)
            return null;
        
        var result = JsonSerializer.Deserialize<UserSession>(data!);
        return result;
    }
}
