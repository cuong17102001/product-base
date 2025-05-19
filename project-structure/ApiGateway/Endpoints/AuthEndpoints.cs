
using Common.DTOs;

namespace ApiGateway.Endpoints;

public static class UserEndpoints
{
    public static void MapUserEndpoints(this WebApplication app, IConfiguration configuration)
    {
        var authServiceUrl = configuration["ServiceUrls:AuthService"];
        var httpClient = app.Services.GetRequiredService<HttpClient>();

        app.MapGet("/api/users/{id}", async (int id) =>
        {
            var response = await httpClient.GetAsync($"{authServiceUrl}/api/user/{id}");
            return await response.Content.ReadAsStringAsync();
        });

        app.MapPost("/api/auth/login", async (LoginDto loginDto) =>
        {
            var response = await httpClient.PostAsJsonAsync($"{authServiceUrl}/api/auth/login", loginDto);
            return await response.Content.ReadAsStringAsync();
        });

        app.MapPost("/api/auth/register", async (RegisterDto registerDto) =>
        {
            var response = await httpClient.PostAsJsonAsync($"{authServiceUrl}/api/auth/register", registerDto);
            return await response.Content.ReadAsStringAsync();
        });

        app.MapGet("/api/users/profile", async (HttpContext context) =>
        {
            var response = await httpClient.GetAsync($"{authServiceUrl}/api/user/profile");
            return await response.Content.ReadAsStringAsync();
        });

        app.MapPut("/api/users/profile", async (UpdateUserDto updateDto, HttpContext context) =>
        {
            var response = await httpClient.PutAsJsonAsync($"{authServiceUrl}/api/user/profile", updateDto);
            return await response.Content.ReadAsStringAsync();
        });
    }
} 