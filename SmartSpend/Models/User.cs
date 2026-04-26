using Microsoft.AspNetCore.Identity;

namespace SmartSpend.Models
{
    public class User : IdentityUser
    {
        public string? Initials { get; set; }
    }
}
