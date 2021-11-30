using MoneyExchangeWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;

namespace MoneyExchangeWebApp.Controllers
{
    public class StockController : Controller
    {
        public IActionResult Index()
        {
            List<Stock> curList = DBUtl.GetList<Stock>("SELECT * FROM Stock");
            return View(curList);
        }
         #region "StockAdd"
        public IActionResult StockEdit()
        {
            return View();
        }

        public IActionResult StockEditPost()
        {
            IFormCollection form = HttpContext.Request.Form;
            string SI = form["Stock_id"].ToString().Trim();
            string SN = form["Stock_name"].ToString().Trim();
            string SA = form["Stock_amount"].ToString().Trim();
            string AR = form["Average_Rate"].ToString().Trim();

            if (ValidUtl.CheckIfEmpty(SI, SN, SA, AR))
            {
                ViewData["Message"] = "Please enter all fields";
                ViewData["MsgType"] = "warning";
                return View("StockEdit");
            }

            if (!SI.IsNumeric())
            {
                ViewData["Message"] = "Stock ID must be integer";
                ViewData["MsgType"] = "warning";
                return View("StockEdit");
            }

            if (!SN.Length.Equals(3))
            {
                ViewData["Message"] = "Stock name can only be 3 letters";
                ViewData["MsgType"] = "warning";
                return View("StockEdit");
            }

                 if (!SA.IsNumeric())
         {
            ViewData["Message"] = "Amount must be an Decimal";
            ViewData["MsgType"] = "warning";
            return View("StockEdit");
         }

                 if (!AR.IsNumeric())
         {
            ViewData["Message"] = "Average Rate must be an Decimal";
            ViewData["MsgType"] = "warning";
            return View("StockEdit");
         }

            string insert_stock = String.Format(@"INSERT INTO Stock(Stock_id, Stock_name, Stock_amount, Average_Rate)
              VALUES('{0}', {1}, {2}, {3})", SI, SN, SA, AR);

            int count = DBUtl.ExecSQL(insert_stock);

            if (count == 1)
            {
                TempData["Message"] = "Stock Successfully Added.";
                TempData["MsgType"] = "success";
                return RedirectToAction("Index");
                
            }
            else
            {
                ViewData["Message"] = DBUtl.DB_Message;
                ViewData["MsgType"] = "danger";
                return View("StockEdit");
            }
        }
        #endregion

        #region "StockDelete"
        public IActionResult StockDelete()
        {
            return View();
        }

        public IActionResult StockDeletePost()
        {
            IFormCollection form = HttpContext.Request.Form;
            string SI = form["Stock_id"].ToString().Trim();

            if (!SI.IsNumeric())
            {
                ViewData["Message"] = "Stock ID must be integer";
                ViewData["MsgType"] = "warning";
                return View("StockDelete");
            }
         

            string select = String.Format(@"SELECT * FROM Stock WHERE Stock_id='{0}'", SI);
            DataTable dt = DBUtl.GetTable(select);
            if (dt.Rows.Count == 0)
            {
                ViewData["Message"] = "Stock id Not Found";
                ViewData["MsgType"] = "warning";
                return View("StockDelete");
            }

            int count = DBUtl.ExecSQL(String.Format(@"DELETE Stock WHERE Stock_id='{0}'", SI));
            if (count == 1)
            {
                TempData["Message"] = "Stock Deleted";
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

   
