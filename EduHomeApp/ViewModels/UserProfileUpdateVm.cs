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
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }
        [DataType(DataType.Password), Compare(nameof(NewPassword))]
        public string? NewRePassword { get; set; }
        public string Password { get; set; }


    }
}
