using Microsoft.AspNetCore.Mvc;

namespace FYP.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
