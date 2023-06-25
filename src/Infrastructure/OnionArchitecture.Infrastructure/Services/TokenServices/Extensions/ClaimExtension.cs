using System.Security.Claims;

namespace OnionArchitecture.Infrastructure.Services.TokenServices.Extensions
{
    public static class ClaimExtension
    {
        public static void AddId(this ICollection<Claim> claims, string id) { claims.Add(new Claim("Id", id)); }
        public static void AddFirstName(this ICollection<Claim> claims, string firstName) { claims.Add(new Claim("FirstName", firstName)); }
        public static void AddLastName(this ICollection<Claim> claims, string lastName) { claims.Add(new Claim("LastName", lastName)); }
        public static void AddEmail(this ICollection<Claim> claims, string email) { claims.Add(new Claim("Email", email)); }
    }
}
