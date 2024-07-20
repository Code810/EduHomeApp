using System.ComponentModel.DataAnnotations;

namespace EduHomeApp.Models
{
    public class HomeContent : BaseEntity
    {
        [StringLength(50)]
        public string Title { get; set; }
        [StringLength(400)]
        public string Description { get; set; }
    }
}
