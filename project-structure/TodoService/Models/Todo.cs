using System;

namespace TodoService.Models;

public class Todo
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public string UserId { get; set; }  // Chỉ lưu UserId
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
} 