using Common.DTOs;

namespace Common.Services;

public interface IUserService
{
    Task<UserDto> GetUserProfileAsync(int userId);
    Task<UserDto> UpdateUserProfileAsync(int userId, UpdateUserDto updateDto);
    Task<AuthResponseDto> LoginAsync(LoginDto loginDto);
    Task<AuthResponseDto> RegisterAsync(RegisterDto registerDto);
} 