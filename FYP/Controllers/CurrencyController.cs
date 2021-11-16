using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using FYP.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace FYP.Controllers
{
    public class CurrencyController : Controller
    {
        public IActionResult Index()
        {
            List<CurrencyExchange> curList = DBUtl.GetList<CurrencyExchange>("SELECT * FROM ExchangeRates");
            return View(curList);
            
        }
    }
}
