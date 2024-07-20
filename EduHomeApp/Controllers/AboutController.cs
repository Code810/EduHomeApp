using EduHomeApp.Data;
using EduHomeApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Controllers
{
    public class AboutController : Controller
    {
        private readonly EduHomeDbContext _context;

        public AboutController(EduHomeDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var teachers = _context.Teachers
          .Include(t => t.TeacherContact)
          .Select(teacher => new TeacherAbouutVm
          {
              Id = teacher.Id,
              ImageUrl = teacher.ImageUrl,
              FullName = teacher.FullName,
              Position = teacher.Position,
              Facebook = teacher.TeacherContact.Facebook,
              Instagram = teacher.TeacherContact.Instagram,
              Pinteres = teacher.TeacherContact.Pinteres,
              Twitter = teacher.TeacherContact.Twitter,
          }).Take(4).ToList();
            var AboutVm = new AboutVm()
            {
                AboutArea = _context.AboutArea.AsNoTracking().SingleOrDefault(),
                TeacherAbouutVm = teachers,
                settings = _context.Settings.ToDictionary(key => key.Key, value => value.Value),
                noticeBoards = _context.NoticeBoards.AsNoTracking().ToList(),

            };
            return View(AboutVm);
        }
    }
}
