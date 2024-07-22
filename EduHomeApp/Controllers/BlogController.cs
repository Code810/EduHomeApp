using EduHomeApp.Data;
using EduHomeApp.Models;
using EduHomeApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Controllers
{
    public class BlogController : Controller
    {
        private readonly EduHomeDbContext _context;

        public BlogController(EduHomeDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            var query = _context.Blogs.AsQueryable();

            return View(await PaginationVm<Blog>.Create(query, page, 3));
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var existBlog = await _context.Blogs
                .FirstOrDefaultAsync(e => e.Id == id);
            if (existBlog == null) return NotFound();

            BlogDetailVm blogtDetail = new()
            {
                Title = existBlog.Title,
                Desc = existBlog.Desc,
                ImageUrl = existBlog.ImageUrl,
                UserId = existBlog.AppUserId,
                CreateDate = existBlog.CreatedDate,
                Categories = await _context.Categories.Include(c => c.Courses)
                .Select(c => new CategoryListVm() { Name = c.Name, Count = c.Courses.Count, Id = c.Id }).ToListAsync()
            };
            return View(blogtDetail);
        }
    }
}
