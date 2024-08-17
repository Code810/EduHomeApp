using EduHomeApp.Areas.AdminArea.ViewModels;
using EduHomeApp.Data;
using EduHomeApp.Extensions;
using EduHomeApp.Helpers;
using EduHomeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "admin, superadmin")]

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
                .ThenInclude(u => u.CourseStudents).ThenInclude(c => c.Course)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return NotFound();
            return View(user);
        }

        public async Task<IActionResult> Update(string? id)
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
                userUpdateVm.TeacherId = user.Teacher.Id;
                userUpdateVm.Relation = "TeacherUpdate";
            }
            return View(userUpdateVm);
        }

        [HttpPost]
        public async Task<IActionResult> Update(string? id, UserUpdateVm userUpdateVm)
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


        public async Task<IActionResult> CreateTeacher(string? id)
        {
            return View();
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> CreateTeacher(string? id, TeacherCreateVm teacherCreateVm)
        {
            if (id == null) return BadRequest();
            if (!ModelState.IsValid) return View();
            var user = await _context.Users.Include(u => u.Teacher).Include(u => u.Student).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null) return BadRequest();
            if (user.Teacher != null && user.Student != null) return BadRequest();
            var file = teacherCreateVm.Photo;
            if (file == null)
            {
                ModelState.AddModelError("Photo", "Shekil bos ola bilmez");
                return View(teacherCreateVm);
            }
            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photos", "Duzgun file secim edin");
                return View(teacherCreateVm);
            }
            if (file.CheckSize(500))
            {
                ModelState.AddModelError("Photos", "faylin olcusu 300kb-dan az olmalidir");
                return View(teacherCreateVm);
            }



            Teacher teacher = new()
            {
                FullName = user.FullName,
                Position = teacherCreateVm.Position,
                About = teacherCreateVm.About,
                Degree = teacherCreateVm.Degree,
                Experience = teacherCreateVm.Experience,
                Hobbies = teacherCreateVm.Hobbies,
                Faculty = teacherCreateVm.Faculty,
                AppUserId = id,
                Language = teacherCreateVm.Language,
                TeamLeader = teacherCreateVm.TeamLeader,
                Development = teacherCreateVm.Development,
                Design = teacherCreateVm.Design,
                Innovation = teacherCreateVm.Innovation,
                Communication = teacherCreateVm.Communication,
                ImageUrl = await file.SaveFile("teacher"),
            };

            TeacherContact teacherContact = new();
            teacherContact.Email = user.Email;
            teacherContact.PhoneNumber = teacherCreateVm.PhoneNumber;
            teacherContact.Skype = teacherCreateVm.Skype;
            teacherContact.Facebook = teacherCreateVm.Facebook;
            teacherContact.Instagram = teacherCreateVm.Instagram;
            teacherContact.Pinteres = teacherCreateVm.Pinteres;
            teacherContact.Twitter = teacherCreateVm.Twitter;
            teacherContact.Teacher = teacher;

            await _userManager.AddToRoleAsync(user, "teacher");
            _context.Teachers.Add(teacher);
            _context.TeacherContacts.Add(teacherContact);
            _context.SaveChanges();
            return RedirectToAction("index");
        }


        public async Task<IActionResult> TeacherUpdate(int id)
        {
            if (id == null) return BadRequest();

            var teacher = await _context.Teachers
                .Include(u => u.TeacherContact)
                 .FirstOrDefaultAsync(u => u.Id == id);
            if (teacher == null) return NotFound();
            TeacherUpdateVm teacherVm = new()
            {
                Position = teacher.Position,
                About = teacher.About,
                Degree = teacher.Degree,
                Experience = teacher.Experience,
                Hobbies = teacher.Hobbies,
                Faculty = teacher.Faculty,
                Language = teacher.Language,
                TeamLeader = teacher.TeamLeader,
                Development = teacher.Development,
                Design = teacher.Design,
                Innovation = teacher.Innovation,
                Communication = teacher.Communication,
                ImageUrl = teacher.ImageUrl,
                PhoneNumber = teacher.TeacherContact.PhoneNumber,
                Skype = teacher.TeacherContact.Skype,
                Facebook = teacher.TeacherContact.Facebook,
                Instagram = teacher.TeacherContact.Instagram,
                Pinteres = teacher.TeacherContact.Pinteres,
                Twitter = teacher.TeacherContact.Twitter,
            };


            return View(teacherVm);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> TeacherUpdate(int? id, TeacherUpdateVm teacherUpdateVm)
        {
            if (id == null) return BadRequest();

            var teacher = await _context.Teachers.Include(t => t.TeacherContact).FirstOrDefaultAsync(t => t.Id == id);
            if (teacher == null) return NotFound();
            teacherUpdateVm.ImageUrl = teacher.ImageUrl;
            if (!ModelState.IsValid) return View(teacherUpdateVm);
            var file = teacherUpdateVm.Photo;
            if (file != null)
            {
                if (!file.CheckContentType())
                {
                    ModelState.AddModelError("Photos", "Duzgun file secim edin");
                    return View(teacherUpdateVm);
                }
                if (file.CheckSize(500))
                {
                    ModelState.AddModelError("Photos", "faylin olcusu 300kb-dan az olmalidir");
                    return View(teacherUpdateVm);
                }
                Helper.DeleteImage("teacher", teacher.ImageUrl);
                teacher.ImageUrl = await file.SaveFile("teacher");
            }
            teacher.Position = teacherUpdateVm.Position;
            teacher.About = teacherUpdateVm.About;
            teacher.Degree = teacherUpdateVm.Degree;
            teacher.Experience = teacherUpdateVm.Experience;
            teacher.Hobbies = teacherUpdateVm.Hobbies;
            teacher.Faculty = teacherUpdateVm.Faculty;
            teacher.Language = teacherUpdateVm.Language;
            teacher.TeamLeader = teacherUpdateVm.TeamLeader;
            teacher.Development = teacherUpdateVm.Development;
            teacher.Design = teacherUpdateVm.Design;
            teacher.Innovation = teacherUpdateVm.Innovation;
            teacher.Communication = teacherUpdateVm.Communication;
            teacher.TeacherContact.PhoneNumber = teacherUpdateVm.PhoneNumber;
            teacher.TeacherContact.Skype = teacherUpdateVm.Skype;
            teacher.TeacherContact.Facebook = teacherUpdateVm.Facebook;
            teacher.TeacherContact.Instagram = teacherUpdateVm.Instagram;
            teacher.TeacherContact.Pinteres = teacherUpdateVm.Pinteres;
            teacher.TeacherContact.Twitter = teacherUpdateVm.Twitter;

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }


}
