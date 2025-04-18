using EventBus.RabbitMQ;
using Logging;
var builder = WebApplication.CreateBuilder(args);
builder.UseCustomSerilog();
builder.WebHost.UseUrls("http://+:5002");
// builder.WebHost.UseUrls("https://+:5002"); https
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
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

app.Run();
