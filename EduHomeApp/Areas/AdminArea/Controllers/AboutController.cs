using EduHomeApp.Areas.AdminArea.ViewModels;
using EduHomeApp.Data;
using EduHomeApp.Extensions;
using EduHomeApp.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class AboutController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public AboutController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            var about = await _dbContext.AboutArea.SingleOrDefaultAsync();
            return View(about);
        }
        public async Task<IActionResult> Detail(int id)
        {
            if (id == null) return BadRequest();

            var about = await _dbContext.AboutArea.FirstOrDefaultAsync(p => p.Id == id);
            if (about == null) return NotFound();
            return View(about);
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id == null) return BadRequest();

            var about = await _dbContext.AboutArea
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            if (about == null) return NotFound();
            AboutUpdateVm aboutUpdateVm = new();
            aboutUpdateVm.Title = about.Title;
            aboutUpdateVm.Description = about.Description;
            aboutUpdateVm.ImageUrl = about.ImageUrl;

            return View(aboutUpdateVm);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, AboutUpdateVm aboutUpdateVm)
        {
            if (id == null) return BadRequest();
            var about = await _dbContext.AboutArea.FirstOrDefaultAsync(s => s.Id == id);
            if (about == null) return NotFound();
            var file = aboutUpdateVm.Photo;
            aboutUpdateVm.ImageUrl = about.ImageUrl;

            if (file != null)
            {
                if (!file.CheckContentType())
                {
                    ModelState.AddModelError("Photo", "Duzgun file secim edin");
                    return View(aboutUpdateVm);
                }
                if (file.CheckSize(500))
                {
                    ModelState.AddModelError("Photo", "faylin olcusu 300kb-dan az olmalidir");
                    return View(aboutUpdateVm);
                }
                string fileName = await file.SaveFile("about");
                Helper.DeleteImage("about", about.ImageUrl);
                about.ImageUrl = fileName;
            }
            about.Title = aboutUpdateVm.Title;
            about.Description = aboutUpdateVm.Description;
            about.UpdatedDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
