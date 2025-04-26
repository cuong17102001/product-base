namespace ApiGateway.Configuration;

public class ServiceConfiguration
{
    public string ServiceName { get; set; }
    public string BaseUrl { get; set; }
    public string HealthCheckEndpoint { get; set; }
    public bool RequiresAuthentication { get; set; }
}

public static class Services
{
    public static readonly ServiceConfiguration AuthService = new()
    {
        ServiceName = "AuthService",
        BaseUrl = "http://localhost:5001",
        HealthCheckEndpoint = "/health",
        RequiresAuthentication = false
    };

    public static readonly ServiceConfiguration UserService = new()
    {
        ServiceName = "UserService",
        BaseUrl = "http://localhost:5002",
        HealthCheckEndpoint = "/health",
        RequiresAuthentication = true
    };

    public static readonly ServiceConfiguration TodoService = new()
    {
        ServiceName = "TodoService",
        BaseUrl = "http://localhost:5003",
        HealthCheckEndpoint = "/health",
        RequiresAuthentication = true
    };

    public static readonly ServiceConfiguration NotificationService = new()
    {
        ServiceName = "NotificationService",
        BaseUrl = "http://localhost:5004",
        HealthCheckEndpoint = "/health",
        RequiresAuthentication = true
    };

    public static readonly List<ServiceConfiguration> AllServices = new()
    {
        AuthService,
        UserService,
        TodoService,
        NotificationService
    };
} 