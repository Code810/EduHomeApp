using EduHomeApp.Data;
using Microsoft.AspNetCore.Mvc;

namespace EduHomeApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class SettingController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public SettingController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var settings = _dbContext.Settings.ToList();

            return View(settings);
        }
    }
}
