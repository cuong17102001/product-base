using Common.DTOs;
using Common.Models;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoService.Data;
using EventBus.Events;

namespace TodoService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly TodoDbContext _context;
    private readonly ILogger<TodoController> _logger;
    private readonly IPublishEndpoint _publishEndpoint;

    public TodoController(TodoDbContext context, ILogger<TodoController> logger, IPublishEndpoint publishEndpoint)
    {
        _context = context;
        _logger = logger;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoResponseDto>>> GetTodos(int userId)
    {
        var todos = await _context.Todos
            .Where(t => t.UserId == userId)
            .Select(t => new TodoResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                UserId = t.UserId,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .ToListAsync();

        return Ok(todos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TodoResponseDto>> GetTodo(int id)
    {
        var todo = await _context.Todos
            .Select(t => new TodoResponseDto
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                IsCompleted = t.IsCompleted,
                UserId = t.UserId,
                CreatedAt = t.CreatedAt,
                UpdatedAt = t.UpdatedAt
            })
            .FirstOrDefaultAsync(t => t.Id == id);

        if (todo == null)
        {
            return NotFound();
        }

        return Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult<TodoResponseDto>> CreateTodo(CreateTodoDto createTodoDto)
    {
        var todo = new Todo
        {
            Title = createTodoDto.Title,
            Description = createTodoDto.Description,
            UserId = 1 // TODO: Get from JWT token
        };

        _context.Todos.Add(todo);
        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new TodoCreatedEvent
        {
            TodoId = todo.Id,
            UserId = todo.UserId,
            Title = todo.Title,
            CreatedAt = todo.CreatedAt
        });

        var response = new TodoResponseDto
        {
            Id = todo.Id,
            Title = todo.Title,
            Description = todo.Description,
            IsCompleted = todo.IsCompleted,
            UserId = todo.UserId,
            CreatedAt = todo.CreatedAt
        };

        return CreatedAtAction(nameof(GetTodo), new { id = todo.Id }, response);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, UpdateTodoDto updateTodoDto)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null)
        {
            return NotFound();
        }

        todo.Title = updateTodoDto.Title;
        todo.Description = updateTodoDto.Description;
        todo.IsCompleted = updateTodoDto.IsCompleted;
        todo.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new TodoUpdatedEvent
        {
            TodoId = todo.Id,
            UserId = todo.UserId,
            Title = todo.Title,
            IsCompleted = todo.IsCompleted,
            UpdatedAt = todo.UpdatedAt.Value
        });

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var todo = await _context.Todos.FindAsync(id);
        if (todo == null)
        {
            return NotFound();
        }

        _context.Todos.Remove(todo);
        await _context.SaveChangesAsync();

        await _publishEndpoint.Publish(new TodoDeletedEvent
        {
            TodoId = todo.Id,
            UserId = todo.UserId
        });

        return NoContent();
    }
} 