using EduHomeApp.Areas.AdminArea.ViewModels;
using EduHomeApp.Data;
using EduHomeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "admin, superadmin")]

    public class CourseLanguageController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public CourseLanguageController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var laguages = _dbContext.CourseLanguages.ToList();
            return View(laguages);
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var laguage = await _dbContext.CourseLanguages.FirstOrDefaultAsync(l => l.Id == id);
            if (laguage == null) return BadRequest();
            CourseLanguageUpdateVm courseLanguageUpdateVm = new();
            courseLanguageUpdateVm.Name = laguage.Name;
            return View(courseLanguageUpdateVm);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, CourseLanguageUpdateVm courseLanguageUpdateVm)
        {
            if (id == null) return BadRequest();
            var language = await _dbContext.CourseLanguages.FirstOrDefaultAsync(l => l.Id == id);
            if (language == null) return NotFound();
            var existCourseLanguage = await _dbContext.CourseLanguages.AnyAsync(e => e.Name == courseLanguageUpdateVm.Name
            && e.Id != id);
            if (existCourseLanguage)
            {
                ModelState.AddModelError("Name", "This language has already been created");
                return View();
            }
            language.Name = courseLanguageUpdateVm.Name;
            language.UpdatedDate = DateTime.Now;
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CourseLanguageCreateVm courseLanguageCreateVm)
        {
            if (!ModelState.IsValid) return View();
            var existLaanguage = await _dbContext.CourseLanguages.AnyAsync(e => e.Name == courseLanguageCreateVm.Name);
            if (existLaanguage)
            {
                ModelState.AddModelError("Name", "This language has already been created");
                return View();
            }
            CourseLanguage courseLanguage = new();
            courseLanguage.Name = courseLanguageCreateVm.Name;
            courseLanguage.CreatedDate = DateTime.Now;
            await _dbContext.CourseLanguages.AddAsync(courseLanguage);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return BadRequest();

            var courseLanguage = await _dbContext.CourseLanguages.FirstOrDefaultAsync(p => p.Id == id);
            if (courseLanguage == null) return NotFound();

            _dbContext.CourseLanguages.Remove(courseLanguage);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
