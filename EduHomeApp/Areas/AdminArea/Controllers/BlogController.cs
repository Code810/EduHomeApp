using EduHomeApp.Areas.AdminArea.ViewModels;
using EduHomeApp.Data;
using EduHomeApp.Extensions;
using EduHomeApp.Models;
using EduHomeApp.ViewModels;
using FiorelloApp.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

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
            var query = _dbContext.Blogs.OrderByDescending(B => B.CreatedDate)
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
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(BlogCreateVm blogCreateVm)
        {
            if (!ModelState.IsValid) return View();
            var file = blogCreateVm.Photo;
            if (file == null)
            {
                ModelState.AddModelError("Photo", "Shekil bos ola bilmez");
                return View(blogCreateVm);
            }
            Blog blog = new();


            if (!file.CheckContentType())
            {
                ModelState.AddModelError("Photos", "Duzgun file secim edin");
                return View(blogCreateVm);
            }
            if (file.CheckSize(500))
            {
                ModelState.AddModelError("Photos", "faylin olcusu 300kb-dan az olmalidir");
                return View(blogCreateVm);
            }
            blog.ImageUrl = await file.SaveFile("blog");
            blog.Title = blogCreateVm.Title;
            blog.Desc = blogCreateVm.Desc;
            blog.CreatedDate = DateTime.Now;
            blog.AppUserId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _dbContext.Blogs.AddAsync(blog);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            if (id == null) return BadRequest();

            var blog = await _dbContext.Blogs
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.Id == id);
            if (blog == null) return NotFound();
            BlogUpdateVm blogUpdateVm = new();
            blogUpdateVm.Title = blog.Title;
            blogUpdateVm.Desc = blog.Desc;
            blogUpdateVm.ImageUrl = blog.ImageUrl;
            return View(blogUpdateVm);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, BlogUpdateVm blogUpdateVm)
        {
            if (id == null) return BadRequest();
            var blog = await _dbContext.Blogs
                .FirstOrDefaultAsync(p => p.Id == id);
            if (blog == null) return NotFound();

            blogUpdateVm.ImageUrl = blog.ImageUrl;
            if (!ModelState.IsValid)
            {
                return View(blogUpdateVm);
            }
            var file = blogUpdateVm.Photo;
            if (file != null)
            {
                if (!file.CheckContentType())
                {
                    ModelState.AddModelError("Photos", "Duzgun file secim edin");
                    return View(blogUpdateVm);
                }
                if (file.CheckSize(500))
                {
                    ModelState.AddModelError("Photos", "faylin olcusu 300kb-dan az olmalidir");
                    return View(blogUpdateVm);
                }
                Helper.DeleteImage("blog", blog.ImageUrl);
                blog.ImageUrl = await file.SaveFile("blog");
            }

            blog.Title = blogUpdateVm.Title;
            blog.Desc = blogUpdateVm.Desc;
            blog.UpdatedDate = DateTime.Now;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return BadRequest();

            var blog = await _dbContext.Blogs.FirstOrDefaultAsync(p => p.Id == id);
            if (blog == null) return NotFound();

            Helper.DeleteImage("blog", blog.ImageUrl);
            _dbContext.Blogs.Remove(blog);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}

