namespace EduHomeApp.ViewModels
{
    public class TeacherDetailVm
    {
        public string FullName { get; set; }
        public string Position { get; set; }
        public string About { get; set; }
        public string Degree { get; set; }
        public decimal Experience { get; set; }
        public string Hobbies { get; set; }
        public string Faculty { get; set; }
        public string ImageUrl { get; set; }
        public ICollection<SkillsListVm> skills { get; set; }
        public TeacherContactListVm contactListVm { get; set; }
    }
}
