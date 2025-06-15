using Microsoft.AspNetCore.Identity;

namespace Project.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string UserType { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
