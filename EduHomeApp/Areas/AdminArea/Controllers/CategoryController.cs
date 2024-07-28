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

    public class CategoryController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public CategoryController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var categories = _dbContext.Categories.ToList();
            return View(categories);
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var category = await _dbContext.Categories.FirstOrDefaultAsync(l => l.Id == id);
            if (category == null) return NotFound();
            CategoryUpdateVm categoryUpdateVm = new();
            categoryUpdateVm.Name = category.Name;
            return View(categoryUpdateVm);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, CategoryUpdateVm categoryUpdateVm)
        {
            if (id == null) return BadRequest();
            var category = await _dbContext.Categories.FirstOrDefaultAsync(l => l.Id == id);
            if (category == null) return NotFound();
            var existCategory = await _dbContext.Categories.AnyAsync(e => e.Name == categoryUpdateVm.Name && e.Id != id);
            if (existCategory)
            {
                ModelState.AddModelError("Name", "This category has already been created");
                return View();
            }
            category.Name = categoryUpdateVm.Name;
            category.UpdatedDate = DateTime.Now;
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(CategoryCreateVm categoryCreateVm)
        {
            if (!ModelState.IsValid) return View();
            var exisCategory = await _dbContext.Categories.AnyAsync(e => e.Name == categoryCreateVm.Name);
            if (exisCategory)
            {
                ModelState.AddModelError("Name", "This category has already been created");
                return View();
            }
            Category category = new();
            category.Name = categoryCreateVm.Name;
            category.CreatedDate = DateTime.Now;
            await _dbContext.Categories.AddAsync(category);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return BadRequest();

            var category = await _dbContext.Categories.FirstOrDefaultAsync(p => p.Id == id);
            if (category == null) return NotFound();

            _dbContext.Categories.Remove(category);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
