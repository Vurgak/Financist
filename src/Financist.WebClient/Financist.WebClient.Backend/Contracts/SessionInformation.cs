namespace Financist.WebClient.Backend.Contracts;

public class SessionInformation
{
    public required string SubjectId { get; init; }
    
    public required string SessionId { get; init; }

    public required int SessionExpiresIn { get; init; }
}
