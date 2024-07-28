using EduHomeApp.Areas.AdminArea.ViewModels;
using EduHomeApp.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "admin, superadmin")]

    public class HomeContentController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public HomeContentController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var homeContent = await _dbContext.HomeContents.AsNoTracking().SingleAsync();
            return View(homeContent);
        }
        public async Task<IActionResult> Update()
        {
            var homeContent = await _dbContext.HomeContents.AsNoTracking().SingleAsync();
            HomeContentUpdateView content = new();
            content.Title = homeContent.Title;
            content.Description = homeContent.Description;
            return View(content);
        }
        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(HomeContentUpdateView contentVm)
        {
            var Content = await _dbContext.HomeContents.SingleAsync();
            if (!ModelState.IsValid) return View();
            Content.Title = contentVm.Title;
            Content.Description = contentVm.Description;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
