namespace EduHomeApp.Areas.AdminArea.ViewModels
{
    public class CourseListVm
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Category { get; set; }
        public string Language { get; set; }
        public decimal Price { get; set; }
        public string TeacherName { get; set; }
    }
}
