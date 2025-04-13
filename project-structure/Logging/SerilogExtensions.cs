using Microsoft.AspNetCore.Builder;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Elasticsearch;
using Serilog.Sinks.Elasticsearch;

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
        var elasticsearchUrl = builder.Configuration["Elasticsearch:Url"] ?? "http://localhost:9200";
        var serviceName = builder.Configuration["ServiceName"] ?? "UnknownService";
        var environment = builder.Environment.EnvironmentName;

        builder.Host.UseSerilog((context, config) =>
        {
            config
                .Enrich.FromLogContext()
                .Enrich.WithProperty("Application", serviceName)
                .Enrich.WithProperty("Environment", environment)
                .Enrich.WithMachineName()
                .Enrich.WithProcessId()
                .Enrich.WithThreadId()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticsearchUrl))
                {
                    AutoRegisterTemplate = true,
                    IndexFormat = $"logs-{serviceName.ToLower()}-{environment.ToLower()}-{DateTime.UtcNow:yyyy.MM.dd}",
                    NumberOfShards = 2,
                    NumberOfReplicas = 1,
                    BatchAction = ElasticOpType.Create,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv7,
                    EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                                     EmitEventFailureHandling.WriteToFailureSink |
                                     EmitEventFailureHandling.RaiseCallback,
                    BufferBaseFilename = "./logs/elastic-buffer",
                    BufferFileSizeLimitBytes = 1024 * 1024 * 2, // 2MB
                    BufferLogShippingInterval = TimeSpan.FromSeconds(5),
                    MinimumLogEventLevel = LogEventLevel.Information,
                    CustomFormatter = new ElasticsearchJsonFormatter(
                        renderMessage: true,
                        inlineFields: true,
                        renderMessageTemplate: false
                    )
                })
                .WriteTo.File(
                    path: $"./logs/{serviceName}-.log",
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 7,
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}");
        });
        
        return builder;
    }
}