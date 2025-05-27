using Microsoft.AspNetCore.Mvc;

namespace TankGameServer.Controllers
{
    public class MatchController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
