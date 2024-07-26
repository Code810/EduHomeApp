using System.ComponentModel.DataAnnotations;

namespace EduHomeApp.ViewModels
{
    public class UserProfileUpdateVm
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string? Email { get; set; }
        public string? newPassword { get; set; }
        public string? newRePassword { get; set; }
        public string Password { get; set; }


    }
}
