using System.ComponentModel.DataAnnotations;

namespace EduHomeApp.Models
{
    public class Slider : BaseEntity
    {
        public string ImageUrl { get; set; }
        [Required, StringLength(50)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
    }
}
