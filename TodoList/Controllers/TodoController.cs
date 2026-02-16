using Microsoft.AspNetCore.Mvc;
using TodoList.Application.Services;

namespace TodoList.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodoController : ControllerBase
{
    private readonly TodoService _service;

    public TodoController(TodoService service)
    {
        _service = service;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
        => Ok(await _service.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Create(string title)
    {
        await _service.CreateAsync(title);
        return Ok();
    }

    [HttpPost("{id}/complete")]
    public async Task<IActionResult> Complete(Guid id)
    {
        await _service.CompleteAsync(id);
        return Ok();
    }
}
