namespace EduHomeApp.Models
{
    public class Event : BaseEntity
    {

        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public List<EventSpeaker> EventSpeakers { get; set; }
    }
}
