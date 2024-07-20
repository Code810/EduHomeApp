namespace EduHomeApp.Models
{
    public class TeacherSkills : BaseEntity
    {
        public int TeacherId { get; set; }
        public Teacher Teacher { get; set; }
        public int SkillsId { get; set; }
        public Skills Skills { get; set; }

    }
}
