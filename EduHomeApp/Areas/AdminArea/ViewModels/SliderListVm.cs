using System.ComponentModel.DataAnnotations;

namespace EduHomeApp.Areas.AdminArea.ViewModels
{
    public class SliderListVm
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        [Required, StringLength(50)]
        public string Title { get; set; }
        [StringLength(200)]
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
