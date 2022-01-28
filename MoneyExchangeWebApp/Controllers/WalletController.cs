using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MoneyExchangeWebApp.Models;
using System;
using System.Collections.Generic;

namespace MoneyExchangeWebApp.Controllers
{
    public class WalletController : Controller
    {
        #region Show Wallet - Karthik
        // GET: Wallet
        [Authorize(Roles = "admin,user")]
        public ActionResult GetWallet()
        {
            if (User.IsInRole("admin"))
            {
                var CurrenciesOwned = DBUtl.GetList<Stock>(string.Format(@"SELECT * FROM Stock WHERE AccountId=1"));
                return Json(new { data = CurrenciesOwned });
            }
            else
            {
                string sql = @"SELECT * FROM Accounts WHERE EmailAddress='{0}'";
                string email = User.Identity.Name;
                List<Account> Acclist = DBUtl.GetList<Account>(string.Format(sql, email.EscQuote()));
                if (Acclist.Count == 1)
                {
                    var CurrenciesOwned = DBUtl.GetList<Stock>(string.Format(@"SELECT * FROM Stock WHERE AccountId={0}", Acclist[0].AccountId));
                    return Json(new { data = CurrenciesOwned });
                }
                else
                {
                    TempData["error"] = "You do not have a wallet!";
                    return RedirectToAction("ExchangeRates", "Currency");
                }
            }
        }

        [Authorize(Roles = "admin,user")]
        public IActionResult ShowWallet(int id)
        {
            var Clist = DBUtl.GetList("SELECT QuoteCurrency FROM ExchangeRates ORDER BY QuoteCurrency");
            ViewData["Currencylist"] = new SelectList(Clist, "QuoteCurrency", "QuoteCurrency");
            if (id == 0)
            {
                return View();

            }
            else
            {
                return RedirectToAction("DepositIntoWallet",id);
            }
        }
        #endregion

        #region Add a Currency - Karthik
        [Authorize(Roles ="admin,user")]
        public IActionResult AddaCurrency(Stock s)
        {
            var Clist = DBUtl.GetList("SELECT QuoteCurrency FROM ExchangeRates ORDER BY QuoteCurrency");
            ViewData["Currencylist"] = new SelectList(Clist, "QuoteCurrency", "QuoteCurrency");

            string StocksOwnedSql = @"SELECT * FROM Stock WHERE AccountId={0} AND ISO='{1}'";
            string updateSql = @"UPDATE Stock SET Amount={0} WHERE StockId={1}";
            string InsertSql = @"INSERT INTO Stock(AccountId, ISO, Amount) VALUES({0}, '{1}', {2})";
            if (User.IsInRole("admin"))
            {
                List<Stock> Slist = DBUtl.GetList<Stock>(String.Format(StocksOwnedSql, 1, s.ISO.EscQuote()));
                if(Slist.Count == 1)
                {
                    double amt = s.Amount + Slist[0].Amount;
                    int res = DBUtl.ExecSQL(String.Format(updateSql, amt, Slist[0].StockId));
                    if(res == 1)
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
                    int res = DBUtl.ExecSQL(String.Format(InsertSql, 1, s.ISO.EscQuote(), s.Amount));
                    if(res == 1)
                    {
                        ViewData["Message"] = "Currency Added!";
                        ViewData["MsgType"] = "success";
                        return View("ShowWallet");
                    }
                    else
                    {
                        ViewData["Message"] = "Currency Not Added to Database!";
                        ViewData["MsgType"] = "danger";
                        return View();
                    }
                }
            }
            else
            {
                string email = User.Identity.Name;
                if (string.IsNullOrEmpty(email))
                {
                    ViewData["Message"] = "You do not have a wallet!";
                    ViewData["MsgType"] = "danger";
                    return View("ShowWallet");
                }
                else
                {
                    List<Account> Alist = DBUtl.GetList<Account>(String.Format(@"SELECT * FROM Accounts WHERE EmailAddress='{0}'",email.EscQuote()));
                    if(Alist.Count == 1)
                    {
                        int AccId = Alist[0].AccountId;
                        List<Stock> Slist = DBUtl.GetList<Stock>(String.Format(StocksOwnedSql, AccId, s.ISO.EscQuote()));
                        if (Slist.Count == 1)
                        {
                            double amt = s.Amount + Slist[0].Amount;
                            int res = DBUtl.ExecSQL(String.Format(updateSql, amt, Slist[0].StockId));
                            if (res == 1)
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
                            int res = DBUtl.ExecSQL(String.Format(InsertSql, AccId, s.ISO.EscQuote(), s.Amount));
                            if (res == 1)
                            {
                                ViewData["Message"] = "Currency Added!";
                                ViewData["MsgType"] = "success";
                                return View("ShowWallet");
                            }
                            else
                            {
                                ViewData["Message"] = "Currency Not Added to Database!";
                                ViewData["MsgType"] = "danger";
                                return View();
                            }
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "Your Account does not exist!";
                        ViewData["MsgType"] = "danger";
                        return View("ShowWallet");
                    }
                }
            }
        }
        #endregion

        #region Deposit Currency - Karthik
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
        public IActionResult DepositIntoWallet(Stock s)
        {
            IFormCollection form = HttpContext.Request.Form;
            string deposit = form["deposit"].ToString().Trim();
            string StocksOwnedSql = @"SELECT * FROM Stock WHERE AccountId={0} AND ISO='{1}'";
            string updateSql = @"UPDATE Stock SET Amount={0} WHERE StockId={1}";

            List<Stock> Slist = DBUtl.GetList<Stock>(String.Format(StocksOwnedSql, 1, s.ISO.EscQuote()));
            if (Slist.Count == 1)
            {
                double amt = + Slist[0].Amount;
                int res = DBUtl.ExecSQL(String.Format(updateSql, amt, Slist[0].StockId));
                if (res == 1)
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
                return NotFound();
            }

        }
        #endregion

        #region For User
        // Display User Waller
        public ActionResult UserWalletIndex()
        {
            List<Account> Accl = DBUtl.GetList<Account>(string.Format("SELECT * FROM Account WHERE EmailAddress='{0}'", User.Identity.Name.EscQuote()));
            string sql = @"SELECT * FROM Stock ";
            return View();
        }

        public IActionResult CheckIfUserHasWallet()
        {
            return View();
        }
        #endregion

        /*#region Unncessary Features DO NOT DELETE - Karthik
        // GET: WalletController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WalletController/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: WalletController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(WalletIndex));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalletController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalletController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(WalletIndex));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalletController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalletController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(WalletIndex));
            }
            catch
            {
                return View();
            }
        }
        #endregion*/

        public IActionResult Login()
        {
            return View();
        }
    }
}
