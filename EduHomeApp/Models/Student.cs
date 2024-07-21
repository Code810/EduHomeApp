namespace EduHomeApp.Models
{
    public class Student
    {
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
