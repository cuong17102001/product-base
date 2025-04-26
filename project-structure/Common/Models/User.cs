using System.ComponentModel.DataAnnotations;

namespace Common.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required] 
    public string Username { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    
    public string? AvatarUrl { get; set; }
    
    public string? Bio { get; set; }
    
    public string? RefreshToken { get; set; }
    
    public DateTime? RefreshTokenExpiryTime { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public DateTime? UpdatedAt { get; set; }
    
    public DateTime? LastActive { get; set; }
} 