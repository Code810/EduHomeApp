namespace EduHomeApp.Models
{
    public class Message : BaseEntity
    {
        public bool IsRead { get; set; }
        public string Subject { get; set; }
        public string MessageText { get; set; }
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }
        public bool IsDelete { get; set; }

    }
}
