using System.ComponentModel.DataAnnotations;

namespace EduHomeApp.Models
{
    public class Teacher : BaseEntity
    {
        [MaxLength(25)]
        public string FullName { get; set; }
        [MaxLength(25)]

        public string Position { get; set; }

        public string About { get; set; }
        public string Degree { get; set; }
        public decimal Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        public string ImageUrl { get; set; }
        public string UserId { get; set; }
        public IEnumerable<TeacherSkills> TeacherSkills { get; set; }
        public TeacherContact TeacherContact { get; set; }
    }
}
