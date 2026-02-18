using Microsoft.EntityFrameworkCore;
using TodoList.Application.Interfaces;
using TodoList.Domain.Entities;
using TodoList.Infrastructure.Data;

namespace TodoList.Infrastructure.Repositories;

public class EfTodoRepository : ITodoRepository
{
    private readonly AppDbContext _context;

    public EfTodoRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(TodoItem todo)
    {
        await _context.Todos.AddAsync(todo);
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Todos.FindAsync(id);
        if (entity == null)
            return;

        _context.Todos.Remove(entity);
    }

    public async Task<List<TodoItem>> GetAllAsync()
    {
        return await _context.Todos.ToListAsync();
    }

    public async Task<TodoItem?> GetByIdAsync(Guid id)
    {
        return await _context.Todos.FindAsync(id);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
