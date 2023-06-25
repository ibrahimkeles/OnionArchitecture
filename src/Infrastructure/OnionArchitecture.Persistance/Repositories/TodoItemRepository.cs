using OnionArchitecture.Application.Repositories;
using OnionArchitecture.Domain.Entites;
using OnionArchitecture.Persistance.Contexts;

namespace OnionArchitecture.Persistance.Repositories
{
    public class TodoItemRepository : Repository<TodoItem>, ITodoItemRepository
    {
        public TodoItemRepository(TodoDbContext todoDbContext) : base(todoDbContext)
        {
        }
    }
}
