namespace EduHomeApp.Models
{
    public class Student : BaseEntity
    {
        public string AppUserId { get; set; }
        public AppUser? AppUser { get; set; }
        public int CourseId { get; set; }
        public Course Course { get; set; }
    }
}
