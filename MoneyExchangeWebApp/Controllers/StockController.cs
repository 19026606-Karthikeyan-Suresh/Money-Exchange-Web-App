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
        #region "View List of stock"
        [Authorize]
        public IActionResult StockIndex()
        {
            List<Stock> curList = DBUtl.GetList<Stock>("SELECT * FROM Stock");
            return View(curList);
        }
        #endregion

        #region "Edit Stock" - Karthik
        [Authorize]
        public IActionResult StockEdit(int id)
        {
            string sql = @"SELECT * FROM Stock WHERE Stock_id={0}";

            string select = String.Format(sql, id);
            List<Stock> Stocklist = DBUtl.GetList<Stock>(select);
            if (Stocklist.Count == 1)
            {
                Stock S = Stocklist[0];
                return View(S);
            }
            else
            {
                TempData["Message"] = "Stock does not exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("StockIndex");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult StockEdit(Stock S)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "danger";
                return View("StockEdit", S);
            }
            else
            {
                string sql = @"UPDATE Stock SET Stock_amount={1} WHERE Stock_id={0}";
                string update = String.Format(sql,S.Stock_id, S.Stock_amount);

                if (DBUtl.ExecSQL(update) == 1)
                {
                    TempData["Message"] = "Stock Updated";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("StockIndex");
            }
        }
        #endregion
    }

}



   
