﻿namespace EduHomeApp.Areas.AdminArea.ViewModels
{
    public class BlogUpdateVm
    {
        public IFormFile? Photo { get; set; }
        public string? ImageUrl { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
    }
}