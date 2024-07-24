namespace EduHomeApp.Areas.AdminArea.ViewModels
{
    public class CourseCreateVm
    {
        public string Name { get; set; }

        public string Duration { get; set; }
        public string ClassDuration { get; set; }
        public string SkillLevel { get; set; }
        public int StudentCapacity { get; set; }
        public string Assesment { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string About { get; set; }
        public string Apply { get; set; }
        public string Certfication { get; set; }
        public IEnumerable<int> TagIds { get; set; }
        public IFormFile Photo { get; set; }
        public DateTime StartDate { get; set; }


        public int CategoryId { get; set; }
        public int TeacherId { get; set; }
        public int CourseLanguageId { get; set; }


    }
}
