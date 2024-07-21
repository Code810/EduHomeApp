using System.ComponentModel.DataAnnotations;

namespace EduHomeApp.ViewModels
{
    public class RegisterVm
    {
        [MaxLength(100)]
        public string UserName { get; set; }
        [MaxLength(100)]
        public string FullName { get; set; }
        [EmailAddress, DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [MaxLength(100), DataType(DataType.Password)]
        public string Password { get; set; }
        [MaxLength(100), DataType(DataType.Password), Compare(nameof(Password))]
        public string RePassword { get; set; }
    }
}
