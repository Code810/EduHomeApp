using EduHomeApp.Data;
using EduHomeApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly EduHomeDbContext _context;

        public HomeController(EduHomeDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            var courses = _context.Courses
            .Include(c => c.Category)
            .Select(course => new CourseHomeVM
            {
                Id = course.Id,
                Category = course.Category.Name,
                ImageUrl = course.ImageUrl,
                Description = course.Description
            }).Take(3).ToList();

            var homeVm = new HomeVM()
            {
                sliders = _context.Sliders.AsNoTracking().ToList(),
                settings = _context.Settings.ToDictionary(key => key.Key, value => value.Value),
                noticeBoards = _context.NoticeBoards.AsNoTracking().ToList(),
                homeContent = _context.HomeContents.AsNoTracking().SingleOrDefault(),
                CourseHomeVMs = courses,
                Events = _context.Events.AsNoTracking().ToList(),
                Blogs = _context.Blogs.AsNoTracking().ToList(),
            };

            return View(homeVm);
        }

    }
}