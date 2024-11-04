using Microsoft.AspNetCore.Identity;

namespace Academy.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int EntityId { get; set; }
        public string? EntityName { get; set; }
    }
}
