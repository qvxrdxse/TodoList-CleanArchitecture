using Microsoft.EntityFrameworkCore;
using TodoList.Domain.Entities;

namespace TodoList.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public DbSet<TodoItem> Todos { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
