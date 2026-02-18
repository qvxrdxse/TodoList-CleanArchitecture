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

    public async Task<List<TodoItem>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task CreateAsync(string title)
    {
        var todo = new TodoItem(title);
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
