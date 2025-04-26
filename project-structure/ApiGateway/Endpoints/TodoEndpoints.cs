using Common.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace ApiGateway.Endpoints;

public static class TodoEndpoints
{
    public static void MapTodoEndpoints(this WebApplication app, IConfiguration configuration)
    {
        var todoServiceUrl = configuration["ServiceUrls:TodoService"];
        var httpClient = app.Services.GetRequiredService<HttpClient>();

        app.MapGet("/api/todos", async () =>
        {
            var response = await httpClient.GetAsync($"{todoServiceUrl}/api/todo");
            return await response.Content.ReadAsStringAsync();
        });

        app.MapGet("/api/todos/{id}", async (int id) =>
        {
            var response = await httpClient.GetAsync($"{todoServiceUrl}/api/todo/{id}");
            return await response.Content.ReadAsStringAsync();
        });

        app.MapPost("/api/todos", async (CreateTodoDto createDto) =>
        {
            var response = await httpClient.PostAsJsonAsync($"{todoServiceUrl}/api/todo", createDto);
            return await response.Content.ReadAsStringAsync();
        });

        app.MapPut("/api/todos/{id}", async (int id, UpdateTodoDto updateDto) =>
        {
            var response = await httpClient.PutAsJsonAsync($"{todoServiceUrl}/api/todo/{id}", updateDto);
            return await response.Content.ReadAsStringAsync();
        });

        app.MapDelete("/api/todos/{id}", async (int id) =>
        {
            var response = await httpClient.DeleteAsync($"{todoServiceUrl}/api/todo/{id}");
            return await response.Content.ReadAsStringAsync();
        });
    }
} 