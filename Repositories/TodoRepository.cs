using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TODOBACKEND.Entities;

namespace TODOBACKEND.Repositories;
public class TodoRepository
{
    private readonly DbContext context;
    private readonly DbSet<Todo> set;
    public TodoRepository(TodoContext context)
    {
        this.context=context;
        set = this.context.Set<Todo>();
    }
    public async Task AddTodo(Todo todo)
    {
        await set.AddAsync(todo);
        await context.SaveChangesAsync();
    }
    public async Task<Todo?> GetTodo(Expression<Func<Todo,bool>> predicate)
    {
        var todo = await set.FirstOrDefaultAsync(predicate);
        return todo;
    }
    public async Task<IEnumerable<Todo>> GetAllTodos(Expression<Func<Todo,bool>> predicate)
    {
        var todo = await set.Where(predicate).ToListAsync();
        return todo;
    }
    public async Task<int> Delete(Expression<Func<Todo, bool>> predicate)
    {
        int affectedRows = await set.Where(predicate).ExecuteDeleteAsync();
        return affectedRows;
    }
    public async Task UpdateTodo(Todo todo)
    {
       set.Entry(todo).State=EntityState.Modified;
       await context.SaveChangesAsync();
    }
}
