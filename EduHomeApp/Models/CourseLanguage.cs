using System.ComponentModel.DataAnnotations;

namespace EduHomeApp.Models
{
    public class CourseLanguage : BaseEntity
    {
        [MaxLength(25)]
        public string Name { get; set; }
        public IEnumerable<Course> Courses { get; set; }
    }
}
