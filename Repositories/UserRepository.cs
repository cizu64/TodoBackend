using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TODOBACKEND.Entities;

namespace TODOBACKEND.Repositories;
public class UserRepository
{
    private readonly DbContext context;
    private readonly DbSet<User> set;
    public UserRepository(TodoContext context)
    {
        this.context=context;
        set = this.context.Set<User>();
    }
    public async Task AddUser(User user)
    {
        await set.AddAsync(user);
        await context.SaveChangesAsync();
    }
    public async Task<User?> GetUser(Expression<Func<User,bool>> predicate)
    {
        var user = await set.FirstOrDefaultAsync(predicate);
        return user;
    }
}
