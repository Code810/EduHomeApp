using EduHomeApp.Data;
using EduHomeApp.Models;
using EduHomeApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Controllers
{
    public class CoursesController : Controller
    {
        private readonly EduHomeDbContext _context;

        public CoursesController(EduHomeDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(int? id)
        {
            ICollection<Course> courses;

            if (id == null)
            {
                courses = _context.Courses
              .Include(c => c.Category)
              .AsNoTracking()
              .ToList();
            }
            else
            {
                courses = _context.Courses.Include(c => c.Category).Where(c => c.CategoryId == id).ToList();
            }

            return View(courses);
        }

        public IActionResult SearchCourse(string? text)
        {

            var courses = _context.Courses
                .Include(c => c.Category)
                .Where(c => c.Description.ToLower().Contains(text) || c.About.ToLower().Contains(text)
                || c.Category.Name.ToLower().Contains(text)).ToList();
            return PartialView("_SearchPartialView", courses);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var existCourse = await _context.Courses
                .Include(c => c.Category)
                .Include(c => c.CourseLanguage)
                .Include(C => C.CourseTags).ThenInclude(C => C.Tag)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (existCourse == null) return NotFound();
            var categories = await _context.Categories.Include(c => c.Courses)
                .Select(c => new CategoryListVm() { Name = c.Name, Count = c.Courses.Count, Id = c.Id }).ToListAsync();



            CourseDetailVm course = new()
            {
                Name = existCourse.Name,
                ImageUrl = existCourse.ImageUrl,
                Category = existCourse.Category.Name,
                StartDate = existCourse.StartDate,
                Duration = existCourse.Duration,
                ClassDuration = existCourse.ClassDuration,
                SkillLevel = existCourse.SkillLevel,
                CourseLanguage = existCourse.CourseLanguage.Name,
                StudentCapacity = existCourse.StudentCapacity,
                Assesment = existCourse.Assesment,
                Price = existCourse.Price,
                Description = existCourse.Description,
                About = existCourse.About,
                Apply = existCourse.Apply,
                Certfication = existCourse.Certfication,
                CourseTags = existCourse.CourseTags,
                CategoryListVms = categories

            };
            return View(course);

        }
    }
}
