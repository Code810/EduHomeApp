using EduHomeApp.Data;
using EduHomeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EduHomeApp.Controllers
{
    [Authorize(Roles = "admin, superadmin,member")]

    public class BuyNowController : Controller
    {
        private readonly EduHomeDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;



        public BuyNowController(EduHomeDbContext context, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var courses = _context.Courses.
                Include(c => c.CourseStudents).ThenInclude(c => c.Student).ThenInclude(s => s.AppUser).ToList();
            courses = courses.Where(c => !c.CourseStudents.Any(s => s.Student.AppUserId == User.FindFirstValue(ClaimTypes.NameIdentifier))).ToList();
            return View(courses);
        }


        public async Task<IActionResult> BuyCourse(int? id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            if (id == null)
                return BadRequest();

            var existCourse = await _context.Courses
                .FirstOrDefaultAsync(c => c.Id == id);
            if (existCourse == null)
                return NotFound();

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var existStudent = await _context.Students
                .Include(s => s.CourseStudents)
                .ThenInclude(cs => cs.Course)
                .FirstOrDefaultAsync(s => s.AppUserId == userId);

            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user.Balans < existCourse.Price)
            {
                ModelState.AddModelError("", "Balansinizda kifayet qeder mebleg yoxdu");
                return View();
            }

            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (existStudent != null)
                    {
                        if (existStudent.CourseStudents.Any(cs => cs.CourseId == id))
                        {
                            ModelState.AddModelError("", "Bu kursu artiq almisiniz");
                            return View();
                        }

                        existStudent.CourseStudents.Add(new CourseStudent { CourseId = existCourse.Id, StudentId = existStudent.Id });
                    }
                    else
                    {
                        var newStudent = new Student
                        {
                            AppUserId = userId,
                            CourseStudents = new List<CourseStudent> { new CourseStudent { CourseId = existCourse.Id } }
                        };

                        _context.Students.Add(newStudent);
                        existStudent = newStudent;
                    }
                    user.Balans -= existCourse.Price;
                    await _userManager.AddToRoleAsync(user, "student");
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    ModelState.AddModelError("", "An error occurred while processing your request.");
                    return View();
                }
            }

            return RedirectToAction("Index", "BuyNow");
        }
    }
}
