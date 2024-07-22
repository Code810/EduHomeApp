using EduHomeApp.Data;
using EduHomeApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FiorelloApp.ViewComponents
{
    public class SettingHeaderViewComponent : ViewComponent
    {
        private readonly EduHomeDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public SettingHeaderViewComponent(EduHomeDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.FullName = user.FullName;
            }

            var settings = _context.Settings.ToDictionary(key => key.Key, value => value.Value);
            return View(await Task.FromResult(settings));
        }
    }
}
