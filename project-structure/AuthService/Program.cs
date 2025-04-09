using Logging;
using EventBus.RabbitMQ;
var builder = WebApplication.CreateBuilder(args);
builder.UseCustomSerilog();
builder.WebHost.UseUrls("http://+:5001");
// builder.WebHost.UseUrls("https://+:5001"); https

// Add services to the container.
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

app.Run();