using Microsoft.AspNetCore.Identity;
using OnionArchitecture.Domain.Entites;

namespace OnionArchitecture.Domain.Identity
{
    public class AppUser : IdentityUser
    {
        public ICollection<TodoList> TodoLists { get; set; }
    }
}
