using Microsoft.EntityFrameworkCore;
using TodoList.Application.Interfaces;
using TodoList.Application.Services;
using TodoList.Infrastructure.Data;
using TodoList.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// ----------------------
// Add services
// ----------------------

builder.Services.AddControllers();

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

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // создаст базу и таблицы, если их нет
}

// ----------------------
// Middleware pipeline
// ----------------------

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
