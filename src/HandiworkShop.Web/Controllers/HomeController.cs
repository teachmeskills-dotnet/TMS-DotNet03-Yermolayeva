using Microsoft.AspNetCore.Mvc;

namespace HandiworkShop.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}