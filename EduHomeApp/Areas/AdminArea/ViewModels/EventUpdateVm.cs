using EduHomeApp.Models;

namespace EduHomeApp.Areas.AdminArea.ViewModels
{
    public class EventUpdateVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public IEnumerable<EventSpeaker>? EventSpeakers { get; set; }
        public IEnumerable<int>? SpeakerIds { get; set; }
        public IFormFile? Photo { get; set; }
        public string? ImageUrl { get; set; }


    }
}
