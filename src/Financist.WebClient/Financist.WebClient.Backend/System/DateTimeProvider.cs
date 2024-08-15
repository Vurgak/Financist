namespace Financist.WebClient.Backend.System;

public class DateTimeOffsetProvider : IDateTimeOffsetProvider
{
    public DateTimeOffset Now => DateTimeOffset.Now; 
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow; 
}
