using System;

namespace NotificationService.DTOs;

public class NotificationDto
{
    public Guid Id { get; set; }
    public string UserId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ReadAt { get; set; }
}

public class CreateNotificationDto
{
    public string UserId { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
}

public class UpdateNotificationDto
{
    public bool IsRead { get; set; }
} 