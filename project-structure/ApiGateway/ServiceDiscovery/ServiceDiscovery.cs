using System.Text.RegularExpressions;

namespace ApiGateway.ServiceDiscovery;

public class ServiceDiscovery
{
    private readonly Dictionary<string, string> _services = new()
    {
        { "auth", "http://localhost:5001" },
        { "todo", "http://localhost:5002" },
        { "notification", "http://localhost:5003" }
    };

    private readonly Dictionary<string, string> _servicePrefixes = new()
    {
        { "auth", "/api/auth" },
        { "todo", "/api/todos" },
        { "notification", "/api/notifications" }
    };

    public string GetServiceUrl(string path)
    {
        foreach (var prefix in _servicePrefixes)
        {
            if (path.StartsWith(prefix.Value, StringComparison.OrdinalIgnoreCase))
            {
                return _services[prefix.Key];
            }
        }

        throw new KeyNotFoundException($"No service found for path: {path}");
    }

    public string GetServicePath(string path)
    {
        foreach (var prefix in _servicePrefixes)
        {
            if (path.StartsWith(prefix.Value, StringComparison.OrdinalIgnoreCase))
            {
                return path;
            }
        }

        throw new KeyNotFoundException($"No service path found for: {path}");
    }
} 