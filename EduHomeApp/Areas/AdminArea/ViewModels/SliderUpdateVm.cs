using System.ComponentModel.DataAnnotations;

namespace EduHomeApp.Areas.AdminArea.ViewModels
{
    public class SliderUpdateVm
    {
        [Required, StringLength(50)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public IFormFile? Photo { get; set; }
        public string? ImageUrl { get; set; }
    }
}
