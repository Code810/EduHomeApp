namespace EduHomeApp.Areas.AdminArea.ViewModels
{
    public class SettingUpdateVm
    {
        public string Key { get; set; }
        public string? Value { get; set; }
        public IFormFile? Photo { get; set; }
        public string? ImageUrl { get; set; }

    }
}
