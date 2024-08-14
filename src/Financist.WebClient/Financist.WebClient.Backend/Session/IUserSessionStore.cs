namespace Financist.WebClient.Backend.Session;

public interface IUserSessionStore
{
    Task SetSessionDataAsync(string sessionId, UserSession session);
    
    Task<UserSession?> GetSessionDataAsync(string sessionId);
}
