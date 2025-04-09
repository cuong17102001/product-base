using Microsoft.AspNetCore.Builder;
using Serilog;

namespace Logging;

public static class SerilogExtensions
{
    // _logger.LogTrace("...");
    // _logger.LogDebug("...");
    // _logger.LogInformation("...");
    // _logger.LogWarning("...");
    // _logger.LogError("...");
    // _logger.LogCritical("...");
    public static WebApplicationBuilder UseCustomSerilog(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, config) =>
        {
            config
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", "AuthService") // hoặc PostService
                .WriteTo.Console()
                .WriteTo.Elasticsearch(new Serilog.Sinks.Elasticsearch.ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = "logstash-{0:yyyy.MM.dd}"
                });
        });
        
        return builder;
    }
}