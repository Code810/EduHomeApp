using System.ComponentModel.DataAnnotations;

namespace EduHomeApp.Areas.AdminArea.ViewModels
{
    public class HomeContentUpdateView
    {
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(400)]
        public string Description { get; set; }
    }
}
