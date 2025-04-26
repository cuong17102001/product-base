using System.Net.Http.Headers;
using System.Text;
using ApiGateway.ServiceDiscovery;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace ApiGateway.Middleware;

public class GatewayMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ServiceDiscovery.ServiceDiscovery _serviceDiscovery;
    private readonly HttpClient _httpClient;
    private readonly ILogger<GatewayMiddleware> _logger;

    public GatewayMiddleware(
        RequestDelegate next,
        ServiceDiscovery.ServiceDiscovery serviceDiscovery,
        IHttpClientFactory httpClientFactory,
        ILogger<GatewayMiddleware> logger)
    {
        _next = next;
        _serviceDiscovery = serviceDiscovery;
        _httpClient = httpClientFactory.CreateClient();
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            var path = context.Request.Path.Value;
            _logger.LogInformation("Received request for path: {Path}", path);

            // Get the target service URL and path
            var serviceUrl = _serviceDiscovery.GetServiceUrl(path);
            var servicePath = _serviceDiscovery.GetServicePath(path);
            var targetUrl = $"{serviceUrl}{servicePath}";

            _logger.LogInformation("Forwarding request to: {TargetUrl}", targetUrl);

            // Create the request message
            var requestMessage = new HttpRequestMessage();
            requestMessage.RequestUri = new Uri(targetUrl);
            requestMessage.Method = new HttpMethod(context.Request.Method);

            // Copy headers
            foreach (var header in context.Request.Headers)
            {
                if (!requestMessage.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray()))
                {
                    requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
                }
            }

            // Copy body if present
            if (context.Request.ContentLength > 0)
            {
                var body = await new StreamReader(context.Request.Body).ReadToEndAsync();
                requestMessage.Content = new StringContent(body, Encoding.UTF8, "application/json");
            }

            // Send the request
            var response = await _httpClient.SendAsync(requestMessage);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Set the response status code
            context.Response.StatusCode = (int)response.StatusCode;

            // Copy response headers
            foreach (var header in response.Headers)
            {
                context.Response.Headers[header.Key] = header.Value.ToArray();
            }

            // Set response content type
            context.Response.ContentType = response.Content.Headers.ContentType?.ToString() ?? "application/json";

            // Write the response body
            await context.Response.WriteAsync(responseContent);

            _logger.LogInformation("Request completed with status code: {StatusCode}", response.StatusCode);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Service not found for path: {Path}", context.Request.Path.Value);
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new { error = "Service not found" }));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing request for path: {Path}", context.Request.Path.Value);
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync(JsonConvert.SerializeObject(new { error = "Internal server error" }));
        }
    }
} 