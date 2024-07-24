using EduHomeApp.Areas.AdminArea.ViewModels;
using EduHomeApp.Data;
using EduHomeApp.Extensions;
using EduHomeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class CourseController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public CourseController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var courses = await _dbContext.Courses
                .Include(c => c.Category)
                .Include(c => c.Teacher)
                .Include(c => c.CourseLanguage)
                .OrderByDescending(B => B.CreatedDate)
                 .Select(c => new CourseListVm()
                 {
                     Name = c.Name,
                     ImageUrl = c.ImageUrl,
                     Id = c.Id,
                     Category = c.Category.Name,
                     Price = c.Price,
                     Language = c.CourseLanguage.Name,
                     TeacherName = c.Teacher.FullName
                 }).ToListAsync();
            return View(courses);
        }
        public async Task<IActionResult> Detail(int id)
        {
            if (id == null) return BadRequest();

            var course = await _dbContext.Courses
                 .Include(c => c.Category)
                .Include(c => c.Teacher)
                .Include(c => c.CourseLanguage)
                .Include(c => c.Students).ThenInclude(s => s.AppUser)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (course == null) return NotFound();
            return View(course);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = new SelectList(await _dbContext.Categories.ToListAsync(), "Id", "Name");
            ViewBag.Teachers = new SelectList(await _dbContext.Teachers.ToListAsync(), "Id", "FullName");
            ViewBag.language = new SelectList(await _dbContext.CourseLanguages.ToListAsync(), "Id", "Name");
            ViewBag.tags = new SelectList(await _dbContext.Tags.ToListAsync(), "Id", "Name");

            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CourseCreateVm courseCreateVm)
        {
            ViewBag.Categories = new SelectList(await _dbContext.Categories.ToListAsync(), "Id", "Name");
            ViewBag.Teachers = new SelectList(await _dbContext.Teachers.ToListAsync(), "Id", "FullName");
            ViewBag.language = new SelectList(await _dbContext.CourseLanguages.ToListAsync(), "Id", "Name");
            ViewBag.tags = new SelectList(await _dbContext.Tags.ToListAsync(), "Id", "Name");

            if (!ModelState.IsValid) return View();
            var file = courseCreateVm.Photo;
            if (file == null)
            {
                ModelState.AddModelError("Photo", "Shekil bos ola bilmez");
                return View(courseCreateVm);
            }
            Course newCourse = new();
            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photos", "Duzgun file secim edin");
                return View(courseCreateVm);
            }
            if (file.CheckSize(500))
            {
                ModelState.AddModelError("Photos", "faylin olcusu 300kb-dan az olmalidir");
                return View(courseCreateVm);
            }

            newCourse.ImageUrl = await file.SaveFile("course");
            newCourse.Name = courseCreateVm.Name;
            newCourse.Duration = courseCreateVm.Duration;
            newCourse.ClassDuration = courseCreateVm.ClassDuration;
            newCourse.SkillLevel = courseCreateVm.SkillLevel;
            newCourse.StudentCapacity = courseCreateVm.StudentCapacity;
            newCourse.StudentCapacity = courseCreateVm.StudentCapacity;
            newCourse.Assesment = courseCreateVm.Assesment;
            newCourse.Price = courseCreateVm.Price;
            newCourse.Description = courseCreateVm.Description;
            newCourse.About = courseCreateVm.About;
            newCourse.Apply = courseCreateVm.Apply;
            newCourse.Certfication = courseCreateVm.Certfication;
            newCourse.StartDate = courseCreateVm.StartDate;
            newCourse.CategoryId = courseCreateVm.CategoryId;
            newCourse.TeacherId = courseCreateVm.TeacherId;
            newCourse.CourseLanguageId = courseCreateVm.CourseLanguageId;
            newCourse.CourseTags = new();
            newCourse.CourseTags.AddRange(courseCreateVm.TagIds.Select(tagId => new CourseTag() { CourseId = newCourse.Id, TagId = tagId }));
            await _dbContext.Courses.AddAsync(newCourse);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Categories = new SelectList(await _dbContext.Categories.ToListAsync(), "Id", "Name");
            ViewBag.Teachers = new SelectList(await _dbContext.Teachers.ToListAsync(), "Id", "FullName");
            ViewBag.language = new SelectList(await _dbContext.CourseLanguages.ToListAsync(), "Id", "Name");
            ViewBag.tags = new SelectList(await _dbContext.Tags.Where(t => !t.CourseTags.Any(t => t.CourseId == id)).ToListAsync(), "Id", "Name");
            if (id == null) return BadRequest();

            var course = await _dbContext.Courses
                .Include(c => c.CourseLanguage)
                .Include(c => c.Category)
                .Include(c => c.Teacher)
                .Include(c => c.CourseTags).ThenInclude(c => c.Tag)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (course == null) return NotFound();
            CourseUpdateVm courseUpdateVm = new();
            courseUpdateVm.Id = course.Id;
            courseUpdateVm.ImageUrl = course.ImageUrl;
            courseUpdateVm.Name = course.Name;
            courseUpdateVm.Duration = course.Duration;
            courseUpdateVm.ClassDuration = course.ClassDuration;
            courseUpdateVm.SkillLevel = course.SkillLevel;
            courseUpdateVm.StudentCapacity = course.StudentCapacity;
            courseUpdateVm.StudentCapacity = course.StudentCapacity;
            courseUpdateVm.Assesment = course.Assesment;
            courseUpdateVm.Price = course.Price;
            courseUpdateVm.Description = course.Description;
            courseUpdateVm.About = course.About;
            courseUpdateVm.Apply = course.Apply;
            courseUpdateVm.Certfication = course.Certfication;
            courseUpdateVm.StartDate = course.StartDate;
            courseUpdateVm.CategoryId = course.Category.Id;
            courseUpdateVm.TeacherId = course.Teacher.Id;
            courseUpdateVm.CourseLanguageId = course.CourseLanguage.Id;
            courseUpdateVm.CourseTags = course.CourseTags;
            return View(courseUpdateVm);
        }

        public async Task<IActionResult> RemoveTags(int? courseId, int? tagId)
        {
            if (courseId == null || tagId == null) return BadRequest();
            var course = await _dbContext.Courses
                .FirstOrDefaultAsync(c => c.Id == courseId);
            if (course == null) return BadRequest();
            var coursetag = await _dbContext.CourseTags.FirstOrDefaultAsync(t => t.TagId == tagId && t.CourseId == courseId);
            _dbContext.CourseTags.Remove(coursetag);
            await _dbContext.SaveChangesAsync();
            return PartialView("_tagPartialView", coursetag);
        }
    }
}
