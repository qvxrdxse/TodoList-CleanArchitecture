using TodoList.Application.DTOs;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;

namespace TodoList.Application.Services;

public class TodoService
{
    private readonly ITodoRepository _repository;

    public TodoService(ITodoRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<TodoResponseDto>> GetAllAsync()
    {
        var todos = await _repository.GetAllAsync();

        return todos.Select(todo => new TodoResponseDto
        {
            Id = todo.Id,
            Title = todo.Title,
            IsCompleted = todo.IsCompleted
        }).ToList();
    }

    public async Task CreateAsync(CreateTodoDto dto)
    {
        var todo = new TodoItem(dto.Title);

        await _repository.AddAsync(todo);
        await _repository.SaveChangesAsync();
    }
    public async Task DeleteAsync(Guid id)
    {
        var todo = await _repository.GetByIdAsync(id);
        if (todo == null)
            throw new Exception("Todo not found");

        await _repository.DeleteAsync(id);
        await _repository.SaveChangesAsync();
    }

    public async Task CompleteAsync(Guid id)
    {
        var todo = await _repository.GetByIdAsync(id);
        if (todo == null)
            throw new Exception("Todo not found");

        todo.Complete();
        await _repository.SaveChangesAsync();
    }
}
