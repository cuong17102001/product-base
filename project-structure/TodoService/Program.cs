using Common.Logging;
using EventBus.RabbitMQ;
using Microsoft.EntityFrameworkCore;
using TodoService.Data;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://+:5003");

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure Serilog using common logging
builder.Host.UseCommonSerilog("TodoService");

// Add DbContext
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add RabbitMQ
builder.Services.AddRabbitMqServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
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
    var db = scope.ServiceProvider.GetRequiredService<TodoDbContext>();
    db.Database.EnsureCreated();
}

app.Run(); 