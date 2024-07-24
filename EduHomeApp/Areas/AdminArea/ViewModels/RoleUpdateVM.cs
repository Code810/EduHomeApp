using Microsoft.AspNetCore.Identity;

namespace EduHomeApp.Areas.AdminArea.ViewModels
{
    public class RoleUpdateVM
    {
        public RoleUpdateVM(string username, List<IdentityRole> roles, IList<string> userRoles)
        {
            Username = username;
            Roles = roles;
            UserRoles = userRoles;
        }

        public string Username { get; set; }
        public List<IdentityRole> Roles { get; set; }
        public IList<string> UserRoles { get; set; }
    }
}
