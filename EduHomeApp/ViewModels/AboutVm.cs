using EduHomeApp.Models;

namespace EduHomeApp.ViewModels
{
    public class AboutVm
    {
        public AboutArea AboutArea { get; set; }
        public IEnumerable<TeacherAbouutVm> TeacherAbouutVm { get; set; }
        public Dictionary<string, string> Settings { get; set; }
        public IEnumerable<NoticeBoard> NoticeBoards { get; set; }


    }
}
