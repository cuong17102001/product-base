using ApiGateway.Endpoints;
using ApiGateway.Middleware;
using ApiGateway.ServiceDiscovery;
using Common.Logging;
using EventBus.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog using common logging
builder.Host.UseCommonSerilog(builder.Configuration["ServiceSettings:ServiceName"]);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add HttpClient
builder.Services.AddHttpClient();

// Add Service Discovery
builder.Services.AddSingleton<ServiceDiscovery>();

// Add RabbitMQ services
builder.Services.AddRabbitMqServices();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Use the Gateway Middleware
app.UseMiddleware<GatewayMiddleware>();

// Map endpoints
app.MapUserEndpoints(builder.Configuration);
app.MapTodoEndpoints(builder.Configuration);

app.Run();
