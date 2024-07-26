using EduHomeApp.Areas.AdminArea.ViewModels;
using EduHomeApp.Data;
using EduHomeApp.Extensions;
using EduHomeApp.Helpers;
using EduHomeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]

    public class SpeakerController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public SpeakerController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var speakers = await _dbContext.Speakers
                .Include(s => s.EventSpeakers).ThenInclude(es => es.Event)
                .OrderByDescending(B => B.CreatedDate)
                 .Select(s => new SpeakerIndexVm()
                 {
                     ImageUrl = s.ImageUrl,
                     Id = s.Id,
                     FullName = s.FullName,
                     Position = s.Position,
                     Events = s.EventSpeakers.Select(s => s.Event.Title.ToString()).ToList()
                 }).ToListAsync();

            return View(speakers);
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id == null) return BadRequest();

            var existSpeaker = await _dbContext.Speakers
                 .Include(c => c.EventSpeakers).ThenInclude(e => e.Event)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (existSpeaker == null) return NotFound();
            SpeakerIndexVm speakerIndexVm = new();
            speakerIndexVm.ImageUrl = existSpeaker.ImageUrl;
            speakerIndexVm.FullName = existSpeaker.FullName;
            speakerIndexVm.Position = existSpeaker.Position;
            speakerIndexVm.Events = existSpeaker.EventSpeakers.Select(s => s.Event.Title.ToString()).ToList();
            return View(speakerIndexVm);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(SpeakerCreateVm speakerCreateVm)
        {

            if (!ModelState.IsValid) return View();
            var file = speakerCreateVm.Photo;
            if (file == null)
            {
                ModelState.AddModelError("Photo", "Shekil bos ola bilmez");
                return View(speakerCreateVm);
            }
            Speaker speaker = new();
            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photos", "Duzgun file secim edin");
                return View(speakerCreateVm);
            }
            if (file.CheckSize(500))
            {
                ModelState.AddModelError("Photos", "faylin olcusu 300kb-dan az olmalidir");
                return View(speakerCreateVm);
            }

            speaker.ImageUrl = await file.SaveFile("event");
            speaker.FullName = speakerCreateVm.FullName;
            speaker.Position = speakerCreateVm.Position;
            await _dbContext.Speakers.AddAsync(speaker);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id == null) return BadRequest();

            var exisSpeaker = await _dbContext.Speakers
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (exisSpeaker == null) return NotFound();

            SpeakerUpdateVm speakerUpdateVm = new();
            speakerUpdateVm.FullName = exisSpeaker.FullName;
            speakerUpdateVm.Position = exisSpeaker.Position;
            speakerUpdateVm.ImageUrl = exisSpeaker.ImageUrl;
            return View(speakerUpdateVm);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, SpeakerUpdateVm speakerUpdateVm)
        {
            var existSpeaker = await _dbContext.Speakers.FirstOrDefaultAsync(p => p.Id == id);
            if (existSpeaker == null) return BadRequest();

            speakerUpdateVm.ImageUrl = existSpeaker.ImageUrl;
            if (!ModelState.IsValid) return View(speakerUpdateVm);
            var file = speakerUpdateVm.Photo;
            if (file != null)
            {
                if (!file.CheckContentType())
                {
                    ModelState.AddModelError("Photos", "Duzgun file secim edin");
                    return View(speakerUpdateVm);
                }
                if (file.CheckSize(500))
                {
                    ModelState.AddModelError("Photos", "faylin olcusu 300kb-dan az olmalidir");
                    return View(speakerUpdateVm);
                }
                Helper.DeleteImage("event", existSpeaker.ImageUrl);
                existSpeaker.ImageUrl = await file.SaveFile("event");
            }
            existSpeaker.FullName = speakerUpdateVm.FullName;
            existSpeaker.Position = speakerUpdateVm.Position;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return BadRequest();

            var existSpeaker = await _dbContext.Speakers.FirstOrDefaultAsync(p => p.Id == id);
            if (existSpeaker == null) return NotFound();

            Helper.DeleteImage("event", existSpeaker.ImageUrl);
            _dbContext.Speakers.Remove(existSpeaker);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
