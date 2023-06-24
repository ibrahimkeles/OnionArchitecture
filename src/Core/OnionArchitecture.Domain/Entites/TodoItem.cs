using OnionArchitecture.Domain.Common;

namespace OnionArchitecture.Domain.Entites
{
    public class TodoItem : BaseEntity
    {
        public int TodoListId { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
        public TodoList TodoList { get; set; }
    }
}
