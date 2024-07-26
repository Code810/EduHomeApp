namespace EduHomeApp.Areas.AdminArea.ViewModels
{
    public class SpeakerIndexVm
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
        public IEnumerable<string> Events { get; set; }
    }
}
