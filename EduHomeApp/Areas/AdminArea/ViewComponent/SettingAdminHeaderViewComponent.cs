

using EduHomeApp.Data;
using EduHomeApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Areas.AdminArea
{
    public class SettingAdminHeaderViewComponent : ViewComponent
    {
        private readonly EduHomeDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public SettingAdminHeaderViewComponent(EduHomeDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            var message = await _context.Messages.Include(m => m.AppUser).ToListAsync();
            return View(await Task.FromResult(message));
        }
    }
}
