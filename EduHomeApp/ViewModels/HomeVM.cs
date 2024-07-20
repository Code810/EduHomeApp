using EduHomeApp.Models;

namespace EduHomeApp.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Slider> sliders { get; set; }
        public Dictionary<string, string> settings { get; set; }
        public IEnumerable<NoticeBoard> noticeBoards { get; set; }
        public HomeContent homeContent { get; set; }
        public IEnumerable<CourseHomeVM> CourseHomeVMs { get; set; }
        public IEnumerable<Event> Events { get; set; }
        public IEnumerable<Blog> Blogs { get; set; }

    }
}
