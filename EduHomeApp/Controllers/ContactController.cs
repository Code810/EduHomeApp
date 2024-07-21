using EduHomeApp.Data;
using Microsoft.AspNetCore.Mvc;
using NuGet.Configuration;

namespace EduHomeApp.Controllers
{
    public class ContactController : Controller
    {
        private readonly EduHomeDbContext _context;

        public ContactController(EduHomeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var settings = _context.Settings.ToDictionary(key => key.Key, value => value.Value);

            return View(settings);
        }
    }
}
