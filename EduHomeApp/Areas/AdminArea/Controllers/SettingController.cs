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


    public class SettingController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public SettingController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            var settings = _dbContext.Settings.ToList();

            return View(settings);
        }
        public async Task<IActionResult> Detail(int id)
        {
            if (id == null) return BadRequest();

            var setting = await _dbContext.Settings.AsNoTracking().Select(s => new SettingListVm()
            {
                Value = s.Value,
                Id = s.Id,
                Key = s.Key,
                CreatedDate = s.CreatedDate,
            }).FirstOrDefaultAsync(s => s.Id == id);
            if (setting == null) return NotFound();
            return View(setting);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(SettingCreateVm settingCreateVm)
        {
            if (!ModelState.IsValid) return View();
            var result = await _dbContext.Settings.AnyAsync(s => s.Key == settingCreateVm.Key);
            if (result)
            {
                ModelState.AddModelError("Key", "Bu adli setting artiq yaradilib");
                return View(settingCreateVm);
            }
            var file = settingCreateVm.Photo;
            if (file == null && settingCreateVm.Value == null)
            {
                ModelState.AddModelError("Photo", "Shekil bos ola bilmez");
                return View(settingCreateVm);
            }
            Setting setting = new();
            if (file != null && settingCreateVm.Value == null)
            {
                if (!file.CheckContentType())
                {
                    ModelState.AddModelError("Photos", "Duzgun file secim edin");
                    return View(settingCreateVm);
                }
                if (file.CheckSize(500))
                {
                    ModelState.AddModelError("Photos", "faylin olcusu 300kb-dan az olmalidir");
                    return View(settingCreateVm);
                }
                setting.Value = await file.SaveFile("logo");
            }
            else
            {
                setting.Value = settingCreateVm.Value;
            }
            setting.Key = settingCreateVm.Key;
            setting.CreatedDate = DateTime.Now;
            await _dbContext.Settings.AddAsync(setting);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();

            var setting = await _dbContext.Settings.AsNoTracking().FirstOrDefaultAsync(s => s.Id == id);
            if (setting == null) return NotFound();

            SettingUpdateVm settingUpdateVm = new();
            settingUpdateVm.Key = setting.Key;
            if (setting.Value.Contains(".jpg") || setting.Value.Contains(".png"))
            {
                settingUpdateVm.ImageUrl = setting.Value;
            }
            else
            {
                settingUpdateVm.Value = setting.Value;
            }

            return View(settingUpdateVm);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, SettingUpdateVm settingUpdateVm)
        {
            if (id == null) return BadRequest();
            var setting = await _dbContext.Settings.FirstOrDefaultAsync(s => s.Id == id);
            var result = await _dbContext.Settings.AnyAsync(s => s.Key == settingUpdateVm.Key && s.Id != id);
            if (result)
            {
                ModelState.AddModelError("Key", "Bu adli setting artiq yaradilib");
                return View(settingUpdateVm);
            }
            if (setting == null) return NotFound();
            if (setting.Value.Contains(".jpg") || setting.Value.Contains(".png"))
            {
                settingUpdateVm.ImageUrl = setting.Value;
            }
            if (!ModelState.IsValid)
            {
                return View(settingUpdateVm);
            }
            var file = settingUpdateVm.Photo;
            if (file != null)
            {
                if (!file.CheckContentType())
                {
                    ModelState.AddModelError("Photos", "Duzgun file secim edin");
                    return View(settingUpdateVm);
                }
                if (file.CheckSize(500))
                {
                    ModelState.AddModelError("Photos", "faylin olcusu 300kb-dan az olmalidir");
                    return View(settingUpdateVm);
                }
                Helper.DeleteImage("logo", setting.Value);
                setting.Value = await file.SaveFile("logo");
            }
            if (settingUpdateVm.Value != null)
            {
                setting.Value = settingUpdateVm.Value;
            }
            setting.Key = settingUpdateVm.Key;
            setting.UpdatedDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var setting = await _dbContext.Settings.FirstOrDefaultAsync(s => s.Id == id);
            if (setting == null) return NotFound();
            if (setting.Value.Contains(".jpg") || setting.Value.Contains(".png"))
            {
                Helper.DeleteImage("logo", setting.Value);
            }
            _dbContext.Settings.Remove(setting);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
