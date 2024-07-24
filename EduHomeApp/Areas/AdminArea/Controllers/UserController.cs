using EduHomeApp.Areas.AdminArea.ViewModels;
using EduHomeApp.Data;
using EduHomeApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class UserController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly EduHomeDbContext _context;

        public UserController(UserManager<AppUser> userManager, EduHomeDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        public async Task<IActionResult> Index()
        {

            var users = await _userManager.Users
                        .Include(u => u.Student)
                        .Include(u => u.Teacher)
                        .ToListAsync();
            return View(users);
        }

        public async Task<IActionResult> SearchUser(string? text, string? value)
        {
            IEnumerable<AppUser> users;
            if (value != null)
            {
                users = await _userManager.GetUsersInRoleAsync(value);
                if (text != null)
                {
                    users = users.Where(u => u.UserName.ToLower().Contains(text.ToLower()) ||
                 u.FullName.ToLower().Contains(text.ToLower()) || u.Email.ToLower().Contains(text.ToLower()));
                }
            }
            else
            {
                users = await _userManager.Users.Where(u => u.UserName.ToLower().Contains(text.ToLower()) ||
               u.FullName.ToLower().Contains(text.ToLower()) || u.Email.ToLower().Contains(text.ToLower())).ToListAsync();
            }
            if (text == null && value == null)
            {
                users = await _userManager.Users.ToListAsync();
            }


            return PartialView("_SearchUserPartialView", users);


        }


        public async Task<IActionResult> ChangeStatus(string id)
        {
            if (id == null) return BadRequest();
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            user.IsBlocked = !user.IsBlocked;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Detail(string? id)
        {
            if (id == null) return BadRequest();
            var user = await _userManager.Users
                .Include(u => u.Teacher).ThenInclude(t => t.TeacherContact)
                .Include(u => u.Student)
                .ThenInclude(u => u.Course)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();
            return View(user);
        }

        public async Task<IActionResult> Update(string id)
        {
            if (id == null) return BadRequest();

            var user = await _userManager.Users
                .Include(u => u.Teacher)
                .Include(u => u.Student)
                 .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();

            UserUpdateVm userUpdateVm = new();
            userUpdateVm.FullName = user.FullName;
            userUpdateVm.Email = user.Email;
            userUpdateVm.UserName = user.UserName;
            if (user.Teacher != null)
            {
                userUpdateVm.Relation = "teacherUpdate";
            }
            if (user.Student != null)
            {
                userUpdateVm.Relation = "studentUpdate";
            }
            return View(userUpdateVm);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(string id, UserUpdateVm userUpdateVm)
        {
            if (id == null) return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            if (!ModelState.IsValid) return View(userUpdateVm);

            user.FullName = userUpdateVm.FullName;
            user.Email = userUpdateVm.Email;
            user.UserName = userUpdateVm.UserName;

            if (!string.IsNullOrEmpty(userUpdateVm.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, userUpdateVm.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(userUpdateVm);
                }
            }

            var resultUpdate = await _userManager.UpdateAsync(user);
            if (!resultUpdate.Succeeded)
            {
                foreach (var error in resultUpdate.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(userUpdateVm);
            }

            await _userManager.UpdateSecurityStampAsync(user);

            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null) return BadRequest();

            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            try
            {
                var result = await _userManager.DeleteAsync(user);
            }
            catch (Exception)
            {

                throw;
            }
            //if (!result.Succeeded)
            //{
            //    foreach (var error in result.Errors)
            //    {
            //        ModelState.AddModelError("", error.Description);
            //    }
            //    return BadRequest(ModelState);
            //}

            return Ok();
        }
    }


}
