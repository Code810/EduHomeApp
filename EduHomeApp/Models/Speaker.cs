namespace EduHomeApp.Models
{
    public class Speaker : BaseEntity
    {
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public IEnumerable<EventSpeaker> EventSpeakers { get; set; }
    }
}
