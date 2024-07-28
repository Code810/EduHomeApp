using EduHomeApp.Areas.AdminArea.ViewModels;
using EduHomeApp.Data;
using EduHomeApp.Extensions;
using EduHomeApp.Helpers;
using EduHomeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "admin, superadmin")]

    public class EventController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public EventController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var courses = await _dbContext.Events
                .Include(c => c.EventSpeakers).ThenInclude(es => es.Speaker)
                .OrderByDescending(B => B.CreatedDate)
                 .Select(c => new EventListVm()
                 {
                     ImageUrl = c.ImageUrl,
                     Id = c.Id,
                     Title = c.Title,
                     Time = c.Time,
                     Address = c.Address,
                     Description = c.Description,
                     Speakers = c.EventSpeakers.Select(s => s.Speaker.FullName.ToString()).ToList()
                 }).ToListAsync();

            return View(courses);
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (id == null) return BadRequest();

            var existEvent = await _dbContext.Events
                 .Include(c => c.EventSpeakers).ThenInclude(e => e.Speaker)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (existEvent == null) return NotFound();
            EventListVm eventListVm = new();
            eventListVm.Title = existEvent.Title;
            eventListVm.Address = existEvent.Address;
            eventListVm.Time = existEvent.Time;
            eventListVm.Description = existEvent.Description;
            eventListVm.ImageUrl = existEvent.ImageUrl;
            eventListVm.Speakers = existEvent.EventSpeakers.Select(s => s.Speaker.FullName.ToString()).ToList();
            return View(eventListVm);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Speakers = new SelectList(await _dbContext.Speakers.ToListAsync(), "Id", "FullName");

            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(EventCreateVm eventCreate)
        {
            ViewBag.Speakers = new SelectList(await _dbContext.Speakers.ToListAsync(), "Id", "FullName");

            if (!ModelState.IsValid) return View();
            var file = eventCreate.Photo;
            if (file == null)
            {
                ModelState.AddModelError("Photo", "Shekil bos ola bilmez");
                return View(eventCreate);
            }
            Event newEvent = new();
            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photos", "Duzgun file secim edin");
                return View(eventCreate);
            }
            if (file.CheckSize(500))
            {
                ModelState.AddModelError("Photos", "faylin olcusu 300kb-dan az olmalidir");
                return View(eventCreate);
            }

            newEvent.ImageUrl = await file.SaveFile("event");
            newEvent.Time = eventCreate.Time;
            newEvent.Title = eventCreate.Title;
            newEvent.Address = eventCreate.Address;
            newEvent.Description = eventCreate.Description;
            newEvent.CreatedDate = DateTime.Now;
            newEvent.EventSpeakers = new();
            newEvent.EventSpeakers.AddRange(eventCreate.SpeakerIds.Select(speakerId => new EventSpeaker() { EventId = newEvent.Id, SpeakerId = speakerId }));
            await _dbContext.Events.AddAsync(newEvent);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id == null) return BadRequest();

            ViewBag.Speakers = new SelectList(await _dbContext.Speakers.Where(s => !s.EventSpeakers.Any(s => s.EventId == id)).ToListAsync(), "Id", "FullName"); ;


            var exisEvent = await _dbContext.Events
                .Include(e => e.EventSpeakers).ThenInclude(e => e.Speaker)
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);

            if (exisEvent == null) return NotFound();

            EventUpdateVm eventUpdateVm = new();
            eventUpdateVm.Title = exisEvent.Title;
            eventUpdateVm.Address = exisEvent.Address;
            eventUpdateVm.Time = exisEvent.Time;
            eventUpdateVm.Description = exisEvent.Description;
            eventUpdateVm.ImageUrl = exisEvent.ImageUrl;
            eventUpdateVm.EventSpeakers = exisEvent.EventSpeakers;
            eventUpdateVm.Id = exisEvent.Id;
            return View(eventUpdateVm);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, EventUpdateVm eventUpdateVm)
        {
            ViewBag.Speakers = new SelectList(await _dbContext.Speakers.Where(s => !s.EventSpeakers.Any(s => s.EventId == id)).ToListAsync(), "Id", "FullName"); ;

            var exisEvent = await _dbContext.Events
                .Include(e => e.EventSpeakers).ThenInclude(e => e.Speaker)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (exisEvent == null) return BadRequest();

            eventUpdateVm.EventSpeakers = exisEvent.EventSpeakers;
            eventUpdateVm.ImageUrl = exisEvent.ImageUrl;
            if (!ModelState.IsValid) return View(eventUpdateVm);
            var file = eventUpdateVm.Photo;
            if (file != null)
            {
                if (!file.CheckContentType())
                {
                    ModelState.AddModelError("Photos", "Duzgun file secim edin");
                    return View(eventUpdateVm);
                }
                if (file.CheckSize(500))
                {
                    ModelState.AddModelError("Photos", "faylin olcusu 300kb-dan az olmalidir");
                    return View(eventUpdateVm);
                }
                Helper.DeleteImage("event", exisEvent.ImageUrl);
                exisEvent.ImageUrl = await file.SaveFile("event");
            }
            exisEvent.Title = eventUpdateVm.Title;
            exisEvent.Address = eventUpdateVm.Address;
            exisEvent.Description = eventUpdateVm.Description;
            exisEvent.Time = eventUpdateVm.Time;

            if (eventUpdateVm.SpeakerIds != null)
            {
                exisEvent.EventSpeakers.AddRange(eventUpdateVm.SpeakerIds.Select(speakerId => new EventSpeaker() { EventId = exisEvent.Id, SpeakerId = speakerId }));
            }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> RemoveSpeakers(int? eventId, int? speakerId)
        {
            if (eventId == null || speakerId == null) return BadRequest();
            var existEvent = await _dbContext.Events.FirstOrDefaultAsync(c => c.Id == eventId);
            if (existEvent == null) return BadRequest();
            var eventSpeaker = await _dbContext.EventSpeakers.FirstOrDefaultAsync(es => es.EventId == eventId && es.SpeakerId == speakerId);
            if (eventSpeaker == null) return BadRequest();
            _dbContext.EventSpeakers.Remove(eventSpeaker);
            await _dbContext.SaveChangesAsync();
            return PartialView("_speakerPartialView", await _dbContext.Speakers.FirstOrDefaultAsync(s => s.Id == speakerId));

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return BadRequest();

            var existEvent = await _dbContext.Events.FirstOrDefaultAsync(p => p.Id == id);
            if (existEvent == null) return NotFound();

            Helper.DeleteImage("event", existEvent.ImageUrl);
            _dbContext.Events.Remove(existEvent);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
