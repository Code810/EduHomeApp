using System.ComponentModel.DataAnnotations;

namespace EduHomeApp.Models
{
    public class Subscribe : BaseEntity
    {
        [EmailAddress]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
