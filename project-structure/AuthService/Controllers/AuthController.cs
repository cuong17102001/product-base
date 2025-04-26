using AuthService.Services;
using Common.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        try
        {
            var result = await _authService.RegisterAsync(registerDto);
            _logger.LogInformation("✅ User registered successfully: {Username}", registerDto.Username);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("❌ Error registering user: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        try
        {
            var result = await _authService.LoginAsync(loginDto);
            _logger.LogInformation("✅ User logged in successfully: {Username}", loginDto.Username);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("❌ Error logging in user: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<AuthResponseDto>> RefreshToken([FromBody] string refreshToken)
    {
        try
        {
            var result = await _authService.RefreshTokenAsync(refreshToken);
            _logger.LogInformation("✅ Token refreshed successfully");
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError("❌ Error refreshing token: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }
}