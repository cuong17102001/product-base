using Common.DTOs;
using Common.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace UserService.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("profile")]
    public async Task<ActionResult<UserDto>> GetProfile()
    {
        try
        {
            var userId = int.Parse(User.FindFirst("sub")?.Value);
            var profile = await _userService.GetUserProfileAsync(userId);
            _logger.LogInformation("✅ Retrieved profile for user {UserId}", userId);
            return Ok(profile);
        }
        catch (Exception ex)
        {
            _logger.LogError("❌ Error getting user profile: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("profile")]
    public async Task<ActionResult<UserDto>> UpdateProfile(UpdateUserDto updateDto)
    {
        try
        {
            var userId = int.Parse(User.FindFirst("sub")?.Value);
            var profile = await _userService.UpdateUserProfileAsync(userId, updateDto);
            _logger.LogInformation("✅ Updated profile for user {UserId}", userId);
            return Ok(profile);
        }
        catch (Exception ex)
        {
            _logger.LogError("❌ Error updating user profile: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Login(LoginDto loginDto)
    {
        try
        {
            var response = await _userService.LoginAsync(loginDto);
            _logger.LogInformation("✅ User logged in successfully: {Email}", loginDto.Username);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("❌ Error during login: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResponseDto>> Register(RegisterDto registerDto)
    {
        try
        {
            var response = await _userService.RegisterAsync(registerDto);
            _logger.LogInformation("✅ User registered successfully: {Email}", registerDto.Email);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError("❌ Error during registration: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
    }
} 