﻿namespace EduHomeApp.ViewModels
{
    public class EventDetailVm
    {
        public string ImageUrl { get; set; }
        public string Title { get; set; }
        public DateTime Time { get; set; }
        public string Address { get; set; }
        public string Description { get; set; }
        public ICollection<SpeakerListVm> Speakers { get; set; }
        public ICollection<CategoryListVm> categories { get; set; }
    }
}