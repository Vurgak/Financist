namespace Financist.WebClient.Backend.Services;

public class EndpointRoutingExclusionService
{
    private readonly IDictionary<string, IEnumerable<string>> _exclusions;
    
    public EndpointRoutingExclusionService(IConfiguration configuration)
    {
        var exclusions = new Dictionary<string, IEnumerable<string>>();
        foreach (var exclusion in configuration.GetSection("ReverseProxy:Exclusions").GetChildren())
        {
            var paths = exclusion.Get<string[]>();
            if (paths is not null)
                exclusions.Add(exclusion.Key, paths);
        }

        _exclusions = exclusions;
    }
    
    public bool IsExcluded(string? service, string path) =>
        service is not null && _exclusions.TryGetValue(service, out var exclusion) && exclusion.Any(path.StartsWith);
}
