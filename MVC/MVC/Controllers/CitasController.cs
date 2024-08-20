using Microsoft.AspNetCore.Mvc;

namespace MVC.Controllers
{
    public class CitasController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
