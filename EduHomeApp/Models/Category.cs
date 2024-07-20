using System.ComponentModel.DataAnnotations;

namespace EduHomeApp.Models
{
    public class Category : BaseEntity
    {
        [MaxLength(25)]
        public string Name { get; set; }
        public List<Course> Courses { get; set; }
    }
}
