using Microsoft.EntityFrameworkCore;

namespace TODOBACKEND.Entities;
public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions options):base(options)
    {
    }

    public DbSet<Todo> Todo{get;set;}
    public DbSet<User> User{get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
