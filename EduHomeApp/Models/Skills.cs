﻿namespace EduHomeApp.Models
{
    public class Skills : BaseEntity
    {
        public int Language { get; set; }
        public int Design { get; set; }
        public int TeamLeader { get; set; }
        public int Innovation { get; set; }
        public int Development { get; set; }
        public int Communication { get; set; }
        public IEnumerable<TeacherSkills> TeacherSkills { get; set; }

    }
}
