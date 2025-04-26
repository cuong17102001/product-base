using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.Services;
using Common.Logging;
using EventBus.RabbitMQ;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Serilog using common logging
builder.Host.UseCommonSerilog(builder.Configuration["ServiceSettings:ServiceName"]);

// Configure DbContext
builder.Services.AddDbContext<NotificationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register services
builder.Services.AddScoped<INotificationService, NotificationService.Services.NotificationService>();

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
app.MapControllers();

// Ensure database is created
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<NotificationDbContext>();
    dbContext.Database.EnsureCreated();
}

app.Run(); 