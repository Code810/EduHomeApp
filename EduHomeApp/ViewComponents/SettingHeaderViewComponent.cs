using EduHomeApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace FiorelloApp.ViewComponents
{
    public class SettingHeaderViewComponent : ViewComponent
    {
        private readonly EduHomeDbContext _context;

        public SettingHeaderViewComponent(EduHomeDbContext context)
        {
            _context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var settings = _context.Settings.ToDictionary(key => key.Key, value => value.Value);
            return View(await Task.FromResult(settings));
        }
    }
}
