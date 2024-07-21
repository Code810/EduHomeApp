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
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int Language { get; set; }
        public int TeamLeader { get; set; }
        public int Development { get; set; }
        public int Design { get; set; }
        public int Innovation { get; set; }
        public int Communication { get; set; }
        public TeacherContact TeacherContact { get; set; }
        public List<Course> Courses { get; set; }
    }
}
