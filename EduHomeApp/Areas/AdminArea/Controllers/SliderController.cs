using EduHomeApp.Areas.AdminArea.ViewModels;
using EduHomeApp.Data;
using EduHomeApp.Extensions;
using EduHomeApp.Helpers;
using EduHomeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "admin, superadmin")]

    public class SliderController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public SliderController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var sliders = await _dbContext.Sliders.AsNoTracking().Select(s => new SliderListVm()
            {
                ImageUrl = s.ImageUrl,
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                CreatedDate = s.CreatedDate,
            }).ToListAsync();
            return View(sliders);
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id == null) return BadRequest();

            var slider = await _dbContext.Sliders.AsNoTracking().Select(s => new SliderListVm()
            {
                ImageUrl = s.ImageUrl,
                Id = s.Id,
                Title = s.Title,
                Description = s.Description,
                CreatedDate = s.CreatedDate,
            }).FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();
            return View(slider);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(SliderCreateVm sliderCreateVm)
        {
            if (!ModelState.IsValid) return View();
            var file = sliderCreateVm.Photo;
            if (file == null)
            {
                ModelState.AddModelError("Photo", "Shekil bos ola bilmez");
                return View(sliderCreateVm);
            }
            Slider slider = new();

            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photos", "Duzgun file secim edin");
                return View(sliderCreateVm);
            }
            if (file.CheckSize(500))
            {
                ModelState.AddModelError("Photos", "faylin olcusu 300kb-dan az olmalidir");
                return View(sliderCreateVm);
            }
            slider.ImageUrl = await file.SaveFile("slider");
            slider.Title = sliderCreateVm.Title;
            slider.Description = sliderCreateVm.Description;
            slider.CreatedDate = DateTime.Now;
            await _dbContext.Sliders.AddAsync(slider);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            var slider = await _dbContext.Sliders.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();

            SliderUpdateVm sliderUpdateVm = new()
            {
                ImageUrl = slider.ImageUrl,
                Title = slider.Title,
                Description = slider.Description
            };
            return View(sliderUpdateVm);

        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, SliderUpdateVm sliderUpdateVm)
        {
            if (id == null) return BadRequest();
            var slider = await _dbContext.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();

            sliderUpdateVm.ImageUrl = slider.ImageUrl;
            if (!ModelState.IsValid)
            {
                return View(sliderUpdateVm);
            }
            var file = sliderUpdateVm.Photo;
            if (file != null)
            {
                if (!file.CheckContentType())
                {
                    ModelState.AddModelError("Photos", "Duzgun file secim edin");
                    return View(sliderUpdateVm);
                }
                if (file.CheckSize(500))
                {
                    ModelState.AddModelError("Photos", "faylin olcusu 300kb-dan az olmalidir");
                    return View(sliderUpdateVm);
                }
                Helper.DeleteImage("slider", slider.ImageUrl);
                slider.ImageUrl = await file.SaveFile("slider");
            }

            slider.Title = sliderUpdateVm.Title;
            slider.Description = sliderUpdateVm.Description;
            slider.UpdatedDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return BadRequest();
            var slider = await _dbContext.Sliders.FirstOrDefaultAsync(s => s.Id == id);
            if (slider == null) return NotFound();
            Helper.DeleteImage("slider", slider.ImageUrl);
            _dbContext.Sliders.Remove(slider);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
