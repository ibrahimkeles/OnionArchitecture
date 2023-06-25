using Microsoft.AspNetCore.Identity;
using OnionArchitecture.Domain.Entites;

namespace OnionArchitecture.Domain.Identity
{
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public ICollection<TodoList> TodoLists { get; set; }
    }
}
