namespace EduHomeApp.Models
{
    public class Blog : BaseEntity
    {
        public string Title { get; set; }
        public string Desc { get; set; }
        public string ImageUrl { get; set; }
        public string? AppUserId { get; set; }
        public AppUser AppUser { get; set; }
    }
}
