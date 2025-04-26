using NotificationService.DTOs;
using NotificationService.Models;

namespace NotificationService.Services;

public interface INotificationService
{
    Task<NotificationDto> GetNotificationByIdAsync(Guid id);
    Task<List<NotificationDto>> GetUserNotificationsAsync(string userId);
    Task<NotificationDto> CreateNotificationAsync(CreateNotificationDto dto);
    Task<NotificationDto> UpdateNotificationAsync(Guid id, UpdateNotificationDto dto);
    Task DeleteNotificationAsync(Guid id);
    Task MarkNotificationAsReadAsync(Guid id);
    Task MarkAllNotificationsAsReadAsync(string userId);
} 