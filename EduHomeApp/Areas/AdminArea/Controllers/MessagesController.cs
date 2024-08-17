using EduHomeApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "admin, superadmin")]
    public class MessagesController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public MessagesController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var Users = await _dbContext.Users.Include(u => u.Messages).ToListAsync();
            return View();
        }
    }
}
