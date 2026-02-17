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
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));

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

// ----------------------
// Middleware pipeline
// ----------------------

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();
