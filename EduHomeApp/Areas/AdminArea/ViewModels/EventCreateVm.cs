namespace EduHomeApp.Areas.AdminArea.ViewModels
{
    public class EventCreateVm
    {
        public IFormFile Photo { get; set; }

        public string Title { get; set; }
        public DateTime Time { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public List<int> SpeakerIds { get; set; }
    }
}
