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

    public class TagController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public TagController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var tags = _dbContext.Tags.ToList();
            return View(tags);
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(l => l.Id == id);
            if (tag == null) return NotFound();
            TagUpdateVm tagUpdateVm = new();
            tagUpdateVm.Name = tag.Name;
            return View(tagUpdateVm);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, TagUpdateVm tagUpdateVm)
        {
            if (id == null) return BadRequest();
            var tag = await _dbContext.Tags.FirstOrDefaultAsync(l => l.Id == id);
            if (tag == null) return NotFound();
            var existTag = await _dbContext.Tags.AnyAsync(e => e.Name == tagUpdateVm.Name && e.Id != id);
            if (existTag)
            {
                ModelState.AddModelError("Name", "This tag has already been created");
                return View();
            }
            tag.Name = tagUpdateVm.Name;
            tag.UpdatedDate = DateTime.Now;
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(TagCreateVm tagCreateVm)
        {
            if (!ModelState.IsValid) return View();
            var exisTag = await _dbContext.Tags.AnyAsync(e => e.Name == tagCreateVm.Name);
            if (exisTag)
            {
                ModelState.AddModelError("Name", "This category has already been created");
                return View();
            }
            Tag tag = new();
            tag.Name = tagCreateVm.Name;
            tag.CreatedDate = DateTime.Now;
            await _dbContext.Tags.AddAsync(tag);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return BadRequest();

            var tag = await _dbContext.Tags.FirstOrDefaultAsync(p => p.Id == id);
            if (tag == null) return NotFound();

            _dbContext.Tags.Remove(tag);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
