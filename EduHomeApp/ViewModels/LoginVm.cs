using System.ComponentModel.DataAnnotations;

namespace EduHomeApp.ViewModels
{
    public class LoginVm
    {
        [MaxLength(100)]
        public string UserNameOrEmail { get; set; }
        [MaxLength(100), DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
