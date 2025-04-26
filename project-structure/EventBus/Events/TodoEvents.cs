namespace EventBus.Events;

public class TodoCreatedEvent
{
    public int TodoId { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class TodoUpdatedEvent
{
    public int TodoId { get; set; }
    public int UserId { get; set; }
    public string Title { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class TodoDeletedEvent
{
    public int TodoId { get; set; }
    public int UserId { get; set; }
} 