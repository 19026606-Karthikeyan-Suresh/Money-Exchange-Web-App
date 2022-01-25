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
    public class DepOrWithController : Controller
    {
        #region "View List of stock" - Karthik
        [Authorize(Roles ="admin, user")]
        public IActionResult StocksOwnedByOwner()
        {
            string email = User.Identity.Name;
            List<Account> Alist = DBUtl.GetList<Account>(String.Format(@"SELECT * FROM Accounts WHERE EmailAddress='{0}'", email.EscQuote()));

            if (Alist.Count == 1)
            {
                var account = Alist[0];
                int AccId = account.AccountId;
                List<Stock> curList = DBUtl.GetList<Stock>(String.Format("SELECT * FROM Stock WHERE AccountId={0}", AccId));
                if (curList.Count == 1)
                {
                    return View(curList);
                }
                else
                {
                    return NotFound();
                }
            }
            else
            {
                return NotFound();
            }

        }
        #endregion

        public IActionResult StockAdd(int id)
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
                string sql1 = @"UPDATE Stock SET Amount={1} WHERE StockId={0}";
                string update = String.Format(sql1,S.StockId, S.Amount);
                string sql2 = @"INSERT INTO DepWithTransactions ";
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



   
