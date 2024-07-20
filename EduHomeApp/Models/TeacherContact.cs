namespace EduHomeApp.Models
{
    public class TeacherContact : BaseEntity
    {
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Skype { get; set; }
        public string Facebook { get; set; }
        public string Instagram { get; set; }
        public string Pinteres { get; set; }
        public string Twitter { get; set; }
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
    }
}
