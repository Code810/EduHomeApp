using EduHomeApp.Data;
using EduHomeApp.Models;
using EduHomeApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    public class BlogController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public BlogController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var query = _dbContext.Blogs
                .AsNoTracking();
            return View(await PaginationVm<Blog>.Create(query, page, 3));
        }
        public async Task<IActionResult> Detail(int id)
        {
            if (id == null) return BadRequest();

            var blog = await _dbContext.Blogs.Include(b => b.AppUser).FirstOrDefaultAsync(p => p.Id == id);
            if (blog == null) return NotFound();
            return View(blog);
        }
    }
}
