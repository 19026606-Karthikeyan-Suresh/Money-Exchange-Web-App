using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyExchangeWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace MoneyExchangeWebApp.Controllers
{
    public class CurrencyController : Controller
    {
        public IActionResult ExchangeRates()
        {
            List<CurrencyExchange> curList = DBUtl.GetList<CurrencyExchange>("SELECT * FROM ExchangeRates");
            return View(curList);

        }

        public IActionResult Index()
        {
            string select =
               @"SELECT Currency_name AS [Currency name],
                     Country AS [Country],
                     Average_Rate AS [Average rate],
                     Created_by AS [Created_by]
                FROM Currency";
            DataTable dt = DBUtl.GetTable(select);
            return View(dt);
        }

        #region "CurrencyEdit"
        public IActionResult CurrencyEdit ()
        {
            return View();
        }

        public IActionResult CurrencyEditPost()
        {
            IFormCollection form = HttpContext.Request.Form;
            string CN = form["Currency_name"].ToString().Trim();
            string C = form["Country"].ToString().Trim();
            string AR = form["Average_Rate"].ToString().Trim();

            if (ValidUtl.CheckIfEmpty(CN, C, AR))
            {
                ViewData["Message"] = "Please enter all fields";
                ViewData["MsgType"] = "warning";
                return View("CurrencyEdit");
            }

            if (!CN.Length.Equals(3))
            {
                ViewData["Message"] = "Currency name can only be 3 letters";
                ViewData["MsgType"] = "warning";
                return View("CurrencyAdd");
            }

            if (!AR.IsNumeric())
            {
                ViewData["Message"] = "Average Rate must be an Decimal";
                ViewData["MsgType"] = "warning";
                return View("CurrencyEdit");
            }

            string insert_currency = String.Format(@"INSERT INTO Currency(Currency_name, Country)
              VALUES('{0}','{1}','{2}')", CN, C, AR);

            int count = DBUtl.ExecSQL(insert_currency);

            if (count == 1)
            {
                TempData["Message"] = "Currency Successfully Edited.";
                TempData["MsgType"] = "success";
                return RedirectToAction("Index");

            }
            else
            {
                ViewData["Message"] = DBUtl.DB_Message;
                ViewData["MsgType"] = "danger";
                return View("CurrencyEdit");
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
            string CN = form["Stock_id"].ToString().Trim();

            if (!CN.Length.Equals(3))
            {
                ViewData["Message"] = "Currency Name must be 3 letters only";
                ViewData["MsgType"] = "warning";
                return View("CurrencyDelete");
            }

            string select = String.Format(@"SELECT * FROM Stock WHERE Stock_id='{0}'", CN);
            DataTable dt = DBUtl.GetTable(select);
            if (dt.Rows.Count == 0)
            {
                ViewData["Message"] = "Stock id Not Found";
                ViewData["MsgType"] = "warning";
                return View("CurrencyDelete");
            }
            else
            {

            }

            int count = DBUtl.ExecSQL(String.Format(@"DELETE Stock WHERE Stock_id='{0}'", CN));
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
            return RedirectToAction("Stock");
        }
        #endregion
    }
}