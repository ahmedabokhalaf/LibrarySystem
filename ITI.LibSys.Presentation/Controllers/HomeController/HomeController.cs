using Microsoft.AspNetCore.Mvc;

namespace ITI.LibSys.Presentation.Controllers.HomeController
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Home Page";
            return View();
        }
        public IActionResult About()
        {
            ViewBag.Title = "About Us";
            return View();
        }
        public IActionResult Contact()
        {
            ViewBag.Title = "Contact Us";
            return View();
        }
    }
}
