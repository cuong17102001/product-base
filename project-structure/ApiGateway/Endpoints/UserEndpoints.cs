using Common.DTOs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Json;

namespace ApiGateway.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app, IConfiguration configuration)
    {
        var userServiceUrl = configuration["ServiceUrls:UserService"];
        var httpClient = app.Services.GetRequiredService<HttpClient>();

        app.MapGet("/api/users/{id}", async (int id) =>
        {
            var response = await httpClient.GetAsync($"{userServiceUrl}/api/user/{id}");
            return await response.Content.ReadAsStringAsync();
        });

        app.MapPost("/api/auth/login", async (LoginDto loginDto) =>
        {
            var response = await httpClient.PostAsJsonAsync($"{userServiceUrl}/api/user/login", loginDto);
            return await response.Content.ReadAsStringAsync();
        });

        app.MapPost("/api/auth/register", async (RegisterDto registerDto) =>
        {
            var response = await httpClient.PostAsJsonAsync($"{userServiceUrl}/api/user/register", registerDto);
            return await response.Content.ReadAsStringAsync();
        });

        app.MapGet("/api/users/profile", async (HttpContext context) =>
        {
            var response = await httpClient.GetAsync($"{userServiceUrl}/api/user/profile");
            return await response.Content.ReadAsStringAsync();
        });

        app.MapPut("/api/users/profile", async (UpdateUserDto updateDto, HttpContext context) =>
        {
            var response = await httpClient.PutAsJsonAsync($"{userServiceUrl}/api/user/profile", updateDto);
            return await response.Content.ReadAsStringAsync();
        });
    }
} 