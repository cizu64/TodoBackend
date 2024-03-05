namespace TODOBACKEND.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TODOBACKEND.Repositories;

[ApiController]
[Route("[controller]/[action]")]
[Authorize]
public class TodoController : ControllerBase
{
    private readonly TodoRepository todoRepository;
    private readonly IConfiguration _configuration;
    public TodoController(TodoRepository todoRepository, IConfiguration configuration)
    {
        this.todoRepository = todoRepository;
        _configuration = configuration;
    }

    [HttpPost]
    public async Task<IActionResult> CreateTodo([FromBody] TodoDTO dto)
    {
        if(!int.TryParse(User.Identity.Name, out int userId))return Unauthorized(new Response{detail="Unauthorized",  statusCode = StatusCodes.Status401Unauthorized});
        await todoRepository.AddTodo(new Entities.Todo
        {
            Title = dto.Title,
            UserId = userId,
            Description = dto.Description,           
        });
        return Ok(new Response{detail = "Todo created!"});
    }

    [HttpGet("{todoId}")]
    public async Task<IActionResult> GetTodo(int todoId)
    {
        if(!int.TryParse(User.Identity.Name, out int userId))return Unauthorized(new Response{detail="Unauthorized", statusCode = StatusCodes.Status401Unauthorized});
        var todo = await todoRepository.GetTodo(t=>t.Id==todoId && t.UserId==userId);
        if(todo is null) return BadRequest(new Response{detail="No Todo found", statusCode = StatusCodes.Status400BadRequest});
        return Ok(new Response{detail = todo});
    }

    [HttpGet]
    public async Task<IActionResult> GetTodos(string? query,int? pageNum=0, int pageSize=5)
    {
        if(!int.TryParse(User.Identity.Name, out int userId))return Unauthorized(new Response{detail="Unauthorized", statusCode = StatusCodes.Status401Unauthorized});
        
        var todos = await todoRepository.GetAllTodos(t=>t.UserId==userId);
        
        var totalPages = todos.Count() / pageSize; 

        if(todos is null) return BadRequest(new Response{detail="No Todos found", statusCode = StatusCodes.Status400BadRequest});

        if(!string.IsNullOrEmpty(query))
        {
            todos = todos.Where(t=>t.Title.ToLower().Contains(query.ToLower()) || t.Description.ToLower().Contains(query.ToLower()));
        }
        if(pageNum!=null)
        {
            todos = todos.Skip(pageNum.Value * pageSize).Take(pageSize);
        }

        return Ok(new Response{detail = todos.Select(t=>new{
            t.Id,
            t.Title,
            t.Description,
            DateCreated = t.DateCreated.ToShortDateString(),
            t.IsComplete
         
        }), meta = new{totalPages}});
    }
    [HttpDelete("{todoId:int}")]
    public async Task<IActionResult> DeleteTodo(int todoId)
    {
        await todoRepository.Delete(t=>t.Id==todoId);
        return Ok(new Response{detail = "Todo deleted"});
    }

    [HttpPut("{todoId:int}")]
    public async Task<IActionResult> CompleteTodo(int todoId, [FromBody] CompleteTodoDTO dto)
    {
        var todo= await todoRepository.GetTodo(t=>t.Id==todoId);
        if(todo is null)return BadRequest(new Response{detail="Todo not found", statusCode = StatusCodes.Status400BadRequest});
        todo.IsComplete = dto.IsComplete;
        await todoRepository.UpdateTodo(todo);
        return Ok(new Response{detail = "Todo updated"});
    }
  
}