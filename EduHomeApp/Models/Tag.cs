using System.ComponentModel.DataAnnotations;

namespace EduHomeApp.Models
{
    public class Tag : BaseEntity
    {
        [MaxLength(25)]
        public string Name { get; set; }
        public IEnumerable<CourseTag> CourseTags { get; set; }
    }
}
