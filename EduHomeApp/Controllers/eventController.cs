using EduHomeApp.Data;
using EduHomeApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Controllers
{
    public class eventController : Controller
    {
        private readonly EduHomeDbContext _context;

        public eventController(EduHomeDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> index()
        {
            var events = await _context.Events
                 .Select(e => new EventHomeVm() { Id = e.Id, ImageUrl = e.ImageUrl, Title = e.Title, Time = e.Time, Description = e.Description, Address = e.Address }).ToListAsync();
            return View(events);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var existEvent = await _context.Events
                .Include(e => e.EventSpeakers).ThenInclude(s => s.Speaker)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (existEvent == null) return NotFound();
            EventDetailVm eventDetail = new()
            {
                ImageUrl = existEvent.ImageUrl,
                Title = existEvent.Title,
                Time = existEvent.Time,
                Description = existEvent.Description,
                Address = existEvent.Address,
                Speakers = existEvent.EventSpeakers.Select(e => new SpeakerListVm()
                { FullName = e.Speaker.FullName, ImageUrl = e.Speaker.ImageUrl, Position = e.Speaker.Position }).ToList(),
                categories = await _context.Categories.Include(c => c.Courses)
                .Select(c => new CategoryListVm() { Name = c.Name, Count = c.Courses.Count, Id = c.Id }).ToListAsync()
            };

            return View(eventDetail);
        }
    }
}
