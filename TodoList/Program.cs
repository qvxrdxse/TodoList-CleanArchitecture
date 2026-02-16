using TodoList.Application.Services;
using TodoList.Application.Interfaces;
using TodoList.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Добавляем контроллеры
builder.Services.AddControllers();

// Регистрируем зависимости
// Services.AddScoped<ITodoRepository, InMemoryTodoRepository>();
builder.Services.AddSingleton<ITodoRepository, InMemoryTodoRepository>();
builder.Services.AddScoped<TodoService>();

// Swagger (удобно для теста API)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Включаем Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
