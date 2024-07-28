using EduHomeApp.Areas.AdminArea.ViewModels;
using EduHomeApp.Data;
using EduHomeApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduHomeApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "admin, superadmin")]

    public class NoticeBoardController : Controller
    {
        private readonly EduHomeDbContext _dbContext;

        public NoticeBoardController(EduHomeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var notice = await _dbContext.NoticeBoards.AsNoTracking().ToListAsync();
            return View(notice);
        }
        public async Task<IActionResult> Detail(int? id)
        {
            if (id == null) return BadRequest();
            var notice = await _dbContext.NoticeBoards.AsNoTracking().FirstOrDefaultAsync(n => n.Id == id);
            if (notice == null) return BadRequest();
            return View(notice);
        }


        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Create(NoticeCreateVm noticeVm)
        {
            if (!ModelState.IsValid) return View();

            NoticeBoard newNotice = new();
            newNotice.Description = noticeVm.Description;
            newNotice.CreatedDate = DateTime.Now;
            await _dbContext.NoticeBoards.AddAsync(newNotice);
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var existNotice = await _dbContext.NoticeBoards.FirstOrDefaultAsync(n => n.Id == id);
            if (existNotice == null) return NotFound();
            NoticeUpdateVm noticeVm = new();
            noticeVm.Description = existNotice.Description;
            return View(noticeVm);
        }


        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Update(int? id, NoticeUpdateVm noticeVm)
        {
            if (id == null) return BadRequest();
            var existNotice = await _dbContext.NoticeBoards.FirstOrDefaultAsync(n => n.Id == id);
            if (existNotice == null) return NotFound();

            existNotice.Description = noticeVm.Description;
            existNotice.UpdatedDate = DateTime.Now;
            _dbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpDelete]
        public async Task<IActionResult> Delete(int? id)
        {

            if (id == null) return BadRequest();

            var notice = await _dbContext.NoticeBoards.FirstOrDefaultAsync(p => p.Id == id);
            if (notice == null) return NotFound();

            _dbContext.NoticeBoards.Remove(notice);
            await _dbContext.SaveChangesAsync();
            return Ok();
        }
    }
}
