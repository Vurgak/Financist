namespace Financist.WebClient.Backend.System;

public interface IDateTimeOffsetProvider
{
    DateTimeOffset Now { get; }
    DateTimeOffset UtcNow { get; }
}