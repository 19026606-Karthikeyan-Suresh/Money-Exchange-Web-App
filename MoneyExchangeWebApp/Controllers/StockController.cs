using Microsoft.AspNetCore.Mvc;
using MoneyExchangeWebApp.Models;
using System.Collections.Generic;

namespace MoneyExchangeWebApp.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            List<Stock> curList = DBUtl.GetList<Stock>("SELECT * FROM Stock");
            return View(curList);
        }
    }
}
