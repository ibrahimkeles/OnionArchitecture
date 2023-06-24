using OnionArchitecture.Domain.Common;
using OnionArchitecture.Domain.Identity;

namespace OnionArchitecture.Domain.Entites
{
    public class TodoList : BaseEntity
    {
        public string UserId { get; set; }
        public string Title { get; set; }
        public AppUser User { get; set; }
        public ICollection<TodoItem> TodoItems { get; set; }

    }
}
