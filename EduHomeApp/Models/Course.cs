namespace EduHomeApp.Models
{
    public class Course : BaseEntity
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public DateTime StartDate { get; set; }
        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public int CourseLanguageId { get; set; }
        public CourseLanguage CourseLanguage { get; set; }
        public int StudentCapacity { get; set; }
        public string Assesment { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
        public string Apply { get; set; }
        public string Certfication { get; set; }
        public IEnumerable<CourseTag> CourseTags { get; set; }
        public IEnumerable<Student> Students { get; set; }

        public int? TeacherId { get; set; }
        public Teacher Teacher { get; set; }


    }
}
