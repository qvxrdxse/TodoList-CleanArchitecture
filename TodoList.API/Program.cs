using Microsoft.EntityFrameworkCore;
using TodoList.Application.Interfaces;
using TodoList.Application.Services;
using TodoList.Infrastructure.Data;
using TodoList.Infrastructure.Repositories;
using FluentValidation;
using FluentValidation.AspNetCore;
using TodoList.Application.Validators;
using TodoList.API.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Add services
// ----------------------

builder.Services.AddControllers();

// Validation
builder.Services.AddFluentValidationAutoValidation(); 
builder.Services.AddValidatorsFromAssemblyContaining<CreateTodoDtoValidator>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// DbContext
var dbPassword = builder.Configuration["DB_PASSWORD"];
var dbServer = builder.Configuration["DB_SERVER"];
var dbName = builder.Configuration["DB_NAME"];

var connectionStringTemplate = builder.Configuration.GetConnectionString("DefaultConnection");
var connectionString = connectionStringTemplate
    .Replace("{DB_PASSWORD}", dbPassword)
    .Replace("{DB_SERVER}", dbServer)
    .Replace("{DB_NAME}", dbName);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// Repository
builder.Services.AddScoped<ITodoRepository, EfTodoRepository>();

// Application services
builder.Services.AddScoped<TodoService>();

// CORS (на будущее для React)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});


// ----------------------
// Middleware pipeline
// ----------------------


var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var logger = services.GetRequiredService<ILogger<Program>>();
    var context = services.GetRequiredService<AppDbContext>(); // Твой класс контекста

    int retries = 10; // Сколько раз пробуем
    int delay = 2000; // Пауза между попытками (2 сек)

    for (int i = 0; i < retries; i++)
    {
        try
        {
            logger.LogInformation("Попытка {Step} из {Total}: подключаемся к БД...", i + 1, retries);

            // Пытаемся накатить миграции
            context.Database.Migrate();

            logger.LogInformation("База данных готова! Таблицы созданы.");
            break; // Если всё ок — выходим из цикла
        }
        catch (Exception ex)
        {
            logger.LogWarning("БД еще не готова. Ждем {Delay}ms... (Ошибка: {Message})", delay, ex.Message);

            if (i == retries - 1) // Если это была последняя попытка
            {
                logger.LogCritical("Не удалось подключиться к БД после {Total} попыток. Всё, приехали.", retries);
                throw; // Вылетаем с ошибкой
            }

            Thread.Sleep(delay); // Ждем и идем на следующий круг
        }
    }
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Todo API V1");
});

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
