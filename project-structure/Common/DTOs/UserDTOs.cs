using System.ComponentModel.DataAnnotations;

namespace Common.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class UpdateUserDto
{
    [EmailAddress]
    public string? Email { get; set; }
    public string? AvatarUrl { get; set; }
    [MaxLength(500)]
    public string? Bio { get; set; }
}

public class UserPreferencesDto
{
    public bool EmailNotifications { get; set; }
    public bool PushNotifications { get; set; }
    public string? TimeZone { get; set; }
    public string? Language { get; set; }
}

public class UserStatisticsDto
{
    public int TotalTodos { get; set; }
    public int CompletedTodos { get; set; }
    public int InProgressTodos { get; set; }
    public DateTime? LastActive { get; set; }
}


