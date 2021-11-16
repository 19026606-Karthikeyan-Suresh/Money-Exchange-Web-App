using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FYP.Controllers
{
    public class SignupController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Thanks()
        {
            IFormCollection form = HttpContext.Request.Form;
            string username = form["Username"].ToString().Trim();
            string password = form["Password"].ToString().Trim();

            ViewData["Username"] = username;
            ViewData["Password"] = password;

            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }
    }
}
