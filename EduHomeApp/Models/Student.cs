namespace EduHomeApp.Models
{
    public class Student : BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public List<CourseStudent> CourseStudents { get; set; }
    }
}
