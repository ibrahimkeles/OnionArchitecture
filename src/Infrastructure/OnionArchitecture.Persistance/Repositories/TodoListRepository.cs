using OnionArchitecture.Application.Repositories;
using OnionArchitecture.Domain.Entites;
using OnionArchitecture.Persistance.Contexts;

namespace OnionArchitecture.Persistance.Repositories
{
    public class TodoListRepository : Repository<TodoList>, ITodoListRepository
    {
        public TodoListRepository(TodoDbContext todoDbContext) : base(todoDbContext)
        {
        }
    }
}
