using EduHomeApp.Data;
using EduHomeApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EduHomeApp.Controllers
{
    public class BuyNowController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly UserManager<AppUser> _userManager;


        public BuyNowController(EduHomeDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IActionResult Index()
        {
            var courses = _context.Courses
                .Include(c => c.Students).ThenInclude(s => s.AppUser).ToList();
            courses = courses.Where(c => !c.Students.Any(s => s.AppUserId == User.FindFirstValue(ClaimTypes.NameIdentifier))).ToList();
            return View(courses);
        }

        [HttpPost]
        public async Task<IActionResult> BuyCourse(int? id)
        {
            if (id == null) return BadRequest();
            var existCourse = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id);
            if (existCourse == null) return NotFound();
            var existStudent = _context.Students.FirstOrDefault(s => s.AppUserId == User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (existStudent != null)
            {
                existStudent.AppUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            }
            return View();

        }
    }
}
