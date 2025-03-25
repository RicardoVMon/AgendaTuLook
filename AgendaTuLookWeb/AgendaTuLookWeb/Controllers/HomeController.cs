using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AgendaTuLookWeb.Models;

namespace AgendaTuLookWeb.Controllers
{
    [FiltroSesion]
	[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
	public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
