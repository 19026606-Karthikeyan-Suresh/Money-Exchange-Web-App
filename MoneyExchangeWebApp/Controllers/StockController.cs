using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoneyExchangeWebApp.Models;
using System;
using System.Collections.Generic;

namespace MoneyExchangeWebApp.Controllers
{
    public class StockController : Controller
    {
        #region Show Wallet - Karthik
        // GET: Wallet
        [Authorize(Roles = "admin")]
        public ActionResult GetWallet()
        {
            var CurrenciesOwned = DBUtl.GetList<Stock>(string.Format(@"SELECT * FROM Stock"));
            return Json(new { data = CurrenciesOwned });
        }

        [Authorize(Roles = "admin")]
        public IActionResult ShowWallet(int id)
        {
            var Clist = DBUtl.GetList("SELECT QuoteCurrency FROM ExchangeRates ORDER BY QuoteCurrency");
            ViewData["Currencylist"] = new SelectList(Clist, "QuoteCurrency", "QuoteCurrency");
            return View();
        }
        #endregion

        #region Add a Currency - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult AddaCurrency(Stock s)
        {
            var Clist = DBUtl.GetList("SELECT QuoteCurrency FROM ExchangeRates ORDER BY QuoteCurrency");
            ViewData["Currencylist"] = new SelectList(Clist, "QuoteCurrency", "QuoteCurrency");

            string StocksOwnedSql = @"SELECT * FROM Stock WHERE ISO='{0}'";
            string updateSql = @"UPDATE Stock SET Amount={0} WHERE StockId={1}";
            string InsertSql = @"INSERT INTO Stock(ISO, Amount) VALUES('{0}', {1})";
            string AddIntoDep = @"INSERT INTO DepWithTransactions(StockId, ISO, DepOrWith, Amount, TransactionDate, Deleted) VALUES({0}, '{1}', 'Deposit', {2}, '{3: yyyy-MM-dd hh:mm:ss}', 0)";
            List<Stock> Slist = DBUtl.GetList<Stock>(String.Format(StocksOwnedSql, s.ISO.EscQuote()));
            if (Slist.Count == 1)
            {
                Stock s1 = Slist[0];
                double amt = s.Amount + s1.Amount;
                int res = DBUtl.ExecSQL(String.Format(updateSql, amt, s1.StockId));
                int res2 = DBUtl.ExecSQL(String.Format(AddIntoDep, s1.StockId, s1.ISO, s1.Amount, DateTime.Now));
                if (res == 1 && res2 == 1)
                {
                    ViewData["Message"] = "Amount has been Updated!";
                    ViewData["MsgType"] = "success";
                    return View("ShowWallet");
                }
                else
                {
                    ViewData["Message"] = "Amount not updated in Database!";
                    ViewData["MsgType"] = "danger";
                    return View();
                }
            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format(InsertSql, s.ISO.EscQuote(), s.Amount));
                if (res == 1)
                {
                    string sql = @"SELECT * FROM Stock WHERE ISO='{0}'";
                    List<Stock> sl = DBUtl.GetList<Stock>(String.Format(sql, s.ISO.EscQuote()));
                    if (sl.Count == 1)
                    {
                        Stock s1 = sl[0];
                        int res2 = DBUtl.ExecSQL(String.Format(AddIntoDep, s1.StockId, s1.ISO, s1.Amount, DateTime.Now));
                        ViewData["Message"] = "Currency Added!";
                        ViewData["MsgType"] = "success";
                        return View("ShowWallet");
                    }
                    else
                    {
                        ViewData["Message"] = DBUtl.DB_Message;
                        ViewData["MsgType"] = "warning";
                        return View();
                    }
                }
                else
                {
                    ViewData["Message"] = "Currency Not Added to Database!";
                    ViewData["MsgType"] = "danger";
                    return View();
                }
            }
        }
        #endregion

        #region Deposit Currency - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult DepositIntoWallet(int id)
        {
            string sql = @"SELECT * FROM Stock WHERE StockId={0}";
            List<Stock> sList = DBUtl.GetList<Stock>(String.Format(sql, id));
            if (sList.Count == 1)
            {
                Stock s = sList[0];
                return View(s);
            }
            else
            {
                ViewData["Message"] = "Currency does not exist";
                return RedirectToAction("ShowWallet");
            }

        }
        [HttpPost]
        [Authorize(Roles = "admin")]
        public IActionResult DepositIntoWallet(Stock s)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Model State is Invalid";
                ViewData["MsgType"] = "danger";
                return View("ShowWallet");
            }
            else
            {
                IFormCollection form = HttpContext.Request.Form;
                string deposit = form["deposit"].ToString().Trim();
                string updateSql = @"UPDATE Stock SET Amount={0} WHERE StockId={1}";
                double myDeposit = Double.Parse(deposit);
                if (s != null)
                {
                    double amt = s.Amount + myDeposit;
                    int res = DBUtl.ExecSQL(String.Format(updateSql, amt, s.StockId));
                    if (res == 1)
                    {
                        string AddIntoDep = @"INSERT INTO DepWithTransactions(StockId, ISO, DepOrWith, Amount, TransactionDate, Deleted) 
                                            VALUES({0}, '{1}', 'Deposit', {2}, '{3: yyyy-MM-dd hh:mm:ss}', 0)";
                        int res1 = DBUtl.ExecSQL(AddIntoDep, s.StockId, s.ISO, myDeposit, DateTime.Now);
                        if (res1 == 1)
                        {
                            ViewData["Message"] = "Amount has been successfully deposited!";
                            ViewData["MsgType"] = "success";
                            return View("ShowWallet");
                        }
                        else
                        {
                            ViewData["Message"] = DBUtl.DB_Message;
                            ViewData["MsgType"] = "danger";
                            return View();
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "Cannot update stock";
                        ViewData["MsgType"] = "danger";
                        return View();
                    }
                }
                else
                {
                    ViewData["Message"] = "You don't possses this stock";
                    ViewData["MsgType"] = "warning";
                    return View("ShowWallet");
                }

            }

        }
        #endregion

        #region Withdraw Currency - Karthik
        public IActionResult WithdrawFromWallet(int id)
        {
            if (User.IsInRole("admin"))
            {
                string sql = @"SELECT * FROM Stock WHERE StockId={0}";
                List<Stock> sList = DBUtl.GetList<Stock>(String.Format(sql, id));
                if (sList.Count == 1)
                {
                    Stock s = sList[0];
                    return View(s);
                }
                else
                {
                    ViewData["Message"] = "Currency does not exist";
                    return RedirectToAction("ShowWallet");
                }
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult WithdrawFromWallet(Stock s)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Model State is Invalid";
                ViewData["MsgType"] = "danger";
                return View("ShowWallet");
            }
            else
            {
                IFormCollection form = HttpContext.Request.Form;
                string withdraw = form["withdraw"].ToString().Trim();
                string updateSql = @"UPDATE Stock SET Amount={0} WHERE StockId={1}";
                double myWithdrawal = Double.Parse(withdraw);
                if (s != null)
                {
                    double amt = s.Amount - myWithdrawal;
                    int res = DBUtl.ExecSQL(String.Format(updateSql, amt, s.StockId));
                    if (res == 1)
                    {
                        ViewData["Message"] = "Amount has been successfully withdrawn!";
                        ViewData["MsgType"] = "success";
                        return View("ShowWallet");
                    }
                    else
                    {
                        ViewData["Message"] = s.StockId;
                        ViewData["MsgType"] = "danger";
                        return View();
                    }
                }
                else
                {
                    return NotFound();
                }

            }
        }
        #endregion
    }
}
