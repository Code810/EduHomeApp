namespace EduHomeApp.Areas.AdminArea.ViewModels
{
    public class EventListVm
    {
        public int Id { get; set; }
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public IEnumerable<string> Speakers { get; set; }

    }
}
