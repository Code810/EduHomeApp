using EduHomeApp.Data;
using EduHomeApp.Models;
using EduHomeApp.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EduHomeApp.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ContactController(EduHomeDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var settings = _context.Settings.ToDictionary(key => key.Key, value => value.Value);
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.FullName = user.FullName;
                ViewBag.Email = user.Email;
            }
            return View(settings);
        }


        [HttpPost]
        public async Task<IActionResult> SendMessage(MessageVm messageVm)
        {
            if (!ModelState.IsValid)
            {
                return Json(new
                {
                    success = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage))
                });
            }

            var user = await _userManager.FindByEmailAsync(messageVm.Email);
            if (user == null)
            {
                return Json(new { success = false, message = "User not found" });
            }

            var message = new Message
            {
                IsRead = false,
                Subject = messageVm.Subject,
                MessageText = messageVm.MessageText,
                AppUserId = user.Id,
                CreatedDate = DateTime.Now,
            };

            try
            {
                await _context.Messages.AddAsync(message);
                await _context.SaveChangesAsync();

                return PartialView("_messagePartialView", message);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "An error occurred while sending the message" });
            }
        }


    }
}
