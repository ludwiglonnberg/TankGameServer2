using Microsoft.AspNetCore.Mvc;

namespace TankGameServer.Controllers
{
    public class StatsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
