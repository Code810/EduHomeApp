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
        public int Language { get; set; }
        public int TeamLeader { get; set; }
        public int Development { get; set; }
        public int Design { get; set; }
        public int Innovation { get; set; }
        public int Communication { get; set; }
        public TeacherContactListVm contactListVm { get; set; }
    }
}
