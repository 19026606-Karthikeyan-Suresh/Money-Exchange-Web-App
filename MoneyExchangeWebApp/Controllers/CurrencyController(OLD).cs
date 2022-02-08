using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Currency.Models;
using System;
using System.Data;

namespace MoneyExchangeWebApp.Controllers
{
    public class CurrencyController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Currency()
        {
            string select =
               @"SELECT Currency_name AS [Currency name],
                     Country AS [Country]
                FROM Currency";
            DataTable dt = DBUtl.GetTable(select);
            return View(dt);
        }

        #region "CurrencyAdd"
        public IActionResult CurrencyAdd()
        {
            return View();
        }

        public IActionResult CurrencyAddPost()
        {
            IFormCollection form = HttpContext.Request.Form;
            string CN = form["Currency_name"].ToString().Trim();
            string C = form["Country"].ToString().Trim();

            if (ValidUtl.CheckIfEmpty(CN, C))
            {
                ViewData["Message"] = "Please enter all fields";
                ViewData["MsgType"] = "warning";
                return View("CurrencyAdd");
            }

            if (!CN.Length.Equals(3))
            {
                ViewData["Message"] = "Currency name can only be 3 letters";
                ViewData["MsgType"] = "warning";
                return View("CurrencyAdd");
            }

            string insert = String.Format(@"INSERT INTO Currency(Currency_name, Country)
              VALUES('{0}','{1}')", CN, C);

            int count = DBUtl.ExecSQL(insert);
            if (count == 1)
            {
                TempData["Message"] = "Currency Successfully Added.";
                TempData["MsgType"] = "success";
                return RedirectToAction("Currency");
            }
            else
            {
                ViewData["Message"] = DBUtl.DB_Message;
                ViewData["MsgType"] = "danger";
                return View("CurrencyAdd");
            }
        }
        #endregion

        #region "CurrencyDelete"
        public IActionResult CurrencyDelete()
        {
            return View();
        }

        public IActionResult CurrencyDeletePost()
        {
                IFormCollection form = HttpContext.Request.Form;
                string CN = form["Currency_name"].ToString().Trim();

            if (!CN.Length.Equals(3))
                {
                    ViewData["Message"] = "Currency Name must be 3 letters only";
                    ViewData["MsgType"] = "warning";   
                    return View("CurrencyDelete");
                }

                string select = String.Format(@"SELECT * FROM Currency WHERE Currency_name='{0}'", CN);
                DataTable dt = DBUtl.GetTable(select);
                if (dt.Rows.Count == 0)
                {
                    ViewData["Message"] = "Currency name Not Found";
                    ViewData["MsgType"] = "warning";
                    return View("CurrencyDelete");
                }

                int count = DBUtl.ExecSQL(String.Format(@"DELETE Currency WHERE Currency_name='{0}'", CN));
                if (count == 1)
                {
                    TempData["Message"] = "Currency Deleted";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                }
                return RedirectToAction("Currency");
        }
        #endregion

    }
}