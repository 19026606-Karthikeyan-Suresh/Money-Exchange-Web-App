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

        public IActionResult CurrencyList()
        {
            string select =
               @"SELECT Stock_id AS [Stock id],
                     Currency_name AS [Currency name],
                     Currency_stock AS [stock],
                     Average_Rate AS [Average rate]
                FROM Stock";
            DataTable dt = DBUtl.GetTable(select);
            return View(dt);
        }

      /*  #region "CurrencyAdd"
        public IActionResult CurrencyAdd()
        {
            return View();
        }

        public IActionResult CurrencyAddPost()
        {
            IFormCollection form = HttpContext.Request.Form;
            string CN = form["Currency_name"].ToString().Trim();
            string C = form["Country"].ToString().Trim();
            decimal A = form["Currency_Stock"];
            decimal AR = form["Average_Rate"];

            if (ValidUtl.CheckIfEmpty(CN, C, A, AR))
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

                 if (!A.IsDecimal())
         {
            ViewData["Message"] = "Amount must be an Decimal";
            ViewData["MsgType"] = "warning";
            return View("CurrencyAdd");
         }

                 if (!AR.IsDecimal())
         {
            ViewData["Message"] = "Average Rate must be an Decimal";
            ViewData["MsgType"] = "warning";
            return View("CurrencyAdd");
         }

            string insert_currency = String.Format(@"INSERT INTO Currency(Currency_name, Country)
              VALUES('{0}','{1}')", CN, C);

            string insert_stock = String.Format(@"INSERT INTO Stock(Currency_name, Currency_Stock, Average_Rate)
              VALUES('{1}', {2}, {3})", CN, A, AR);

            int count = DBUtl.ExecSQL(insert_currency);
            int count1 = DBUtl.ExecSQL(insert_stock);

            if (count == 1 && count == 1)
            {
                TempData["Message"] = "Currency Successfully Added.";
                TempData["MsgType"] = "success";
                return RedirectToAction("CurrencyList");
                
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
        #endregion*/
    }
}

