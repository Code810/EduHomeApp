using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EduHomeApp.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "admin, superadmin")]

    public class DashBoardController : Controller

    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
