using TodoList.Domain.Entities;

namespace TodoList.Application.Interfaces;

public interface ITodoRepository
{
    Task<List<TodoItem>> GetAllAsync();
    Task<TodoItem?> GetByIdAsync(Guid id);
    Task AddAsync(TodoItem item);
    Task SaveChangesAsync();
}
