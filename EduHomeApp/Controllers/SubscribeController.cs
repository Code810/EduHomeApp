using EduHomeApp.Data;
using EduHomeApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace EduHomeApp.Controllers
{
    public class SubscribeController : Controller
    {
        private readonly EduHomeDbContext _context;

        public SubscribeController(EduHomeDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> SubscribeEmail(string email)
        {
            if (email == null) return BadRequest("Please write an email address.");
            if (!ModelState.IsValid) return BadRequest("Please write a valid email address.");
            if (_context.Subscribes.Any(e => e.Email == email))
                return BadRequest("This email address is already subscribed");

            Subscribe subscribe = new() { CreatedDate = DateTime.Now, Email = email };
            await _context.Subscribes.AddAsync(subscribe);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
