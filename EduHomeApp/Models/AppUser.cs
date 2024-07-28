using Microsoft.AspNetCore.Identity;

namespace EduHomeApp.Models
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }
        public bool IsBlocked { get; set; }
        public Teacher? Teacher { get; set; }
        public Student? Student { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }
        public decimal? Balans { get; set; }
        public IEnumerable<Message> Messages { get; set; }
    }
}
