using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnionArchitecture.Domain.Entites;
using OnionArchitecture.Domain.Identity;

namespace OnionArchitecture.Persistance.Contexts
{
    public class TodoDbContext : IdentityDbContext<AppUser, AppRole, string>
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> dbContext) : base(dbContext) { }
        public DbSet<TodoItem> TodoItems { get; set; }
        public DbSet<TodoList> TodoLists { get; set; }
    }
}
