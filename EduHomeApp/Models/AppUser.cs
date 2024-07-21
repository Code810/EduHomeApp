using Microsoft.AspNetCore.Identity;

namespace EduHomeApp.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsBlocked { get; set; }
        public Teacher Teacher { get; set; }
    }
}
