using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;

namespace TodoList.Infrastructure.Repositories;

public class InMemoryTodoRepository : ITodoRepository
{
    private readonly List<TodoItem> _items = new();

    public Task<List<TodoItem>> GetAllAsync()
        => Task.FromResult(_items);

    public Task<TodoItem?> GetByIdAsync(Guid id)
        => Task.FromResult(_items.FirstOrDefault(x => x.Id == id));

    public Task AddAsync(TodoItem item)
    {
        _items.Add(item);
        return Task.CompletedTask;
    }

    public Task SaveChangesAsync()
        => Task.CompletedTask;

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
