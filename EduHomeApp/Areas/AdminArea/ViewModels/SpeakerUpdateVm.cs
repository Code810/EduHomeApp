namespace EduHomeApp.Areas.AdminArea.ViewModels
{
    public class SpeakerUpdateVm
    {
        public IFormFile? Photo { get; set; }
        public string? ImageUrl { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
    }
}
