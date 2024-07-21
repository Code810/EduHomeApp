using EduHomeApp.Data;
using EduHomeApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Controllers
{
    public class TeacherController : Controller
    {
        private readonly EduHomeDbContext _context;

        public TeacherController(EduHomeDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var teachers = await _context.Teachers
                .Include(t => t.TeacherContact)
                .Select(e => new TeacherHomeVm()
                {
                    Id = e.Id,
                    FullName = e.FullName,
                    ImageUrl = e.ImageUrl,
                    Position = e.Position,
                    Facebook = e.TeacherContact.Facebook,
                    Instagram = e.TeacherContact.Instagram,
                    Twitter = e.TeacherContact.Twitter,
                    Pinteres = e.TeacherContact.Pinteres,
                }).ToListAsync();
            return View(teachers);
        }

        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var existTeacher = await _context.Teachers
                .Include(t => t.TeacherContact)
                .FirstOrDefaultAsync(e => e.Id == id);
            if (existTeacher == null) return NotFound();

            TeacherDetailVm teacherDetailVm = new()
            {
                FullName = existTeacher.FullName,
                About = existTeacher.About,
                Position = existTeacher.Position,
                Degree = existTeacher.Degree,
                Experience = existTeacher.Experience,
                Hobbies = existTeacher.Hobbies,
                Faculty = existTeacher.Faculty,
                ImageUrl = existTeacher.ImageUrl,
                Language = existTeacher.Language,
                TeamLeader = existTeacher.TeamLeader,
                Design = existTeacher.Design,
                Development = existTeacher.Development,
                Innovation = existTeacher.Innovation,
                Communication = existTeacher.Communication,


                contactListVm = new()
                {
                    Email = existTeacher.TeacherContact.Email,
                    PhoneNumber = existTeacher.TeacherContact.PhoneNumber,
                    Skype = existTeacher.TeacherContact.Skype,
                    Facebook = existTeacher.TeacherContact.Facebook,
                    Instagram = existTeacher.TeacherContact.Instagram,
                    Pinteres = existTeacher.TeacherContact.Pinteres,
                    Twitter = existTeacher.TeacherContact.Twitter
                }
            };

            return View(teacherDetailVm);
        }
    }
}
