using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MoneyExchangeWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Data;

namespace MoneyExchangeWebApp.Controllers
{
    public class CurrencyTradeController : Controller
    {
        #region "View All Currency Trades" - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult CurrencyTradeIndex()
        {
            string sql = @"SELECT * FROM CurrenyTrades WHERE Deleted='False'";
            List<CurrencyTrade> CTRlist = DBUtl.GetList<CurrencyTrade>(sql);
            return View("CurrencyTradeIndex", CTRlist);
        }
        #endregion

        #region "View All Deleted Currency Trades" - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult DeletedCurrencyTrades()
        {
            List<CurrencyTrade> tranList = DBUtl.GetList<CurrencyTrade>("SELECT * FROM ConvTransactions WHERE Deleted='True' ORDER BY TransactionDate DESC");
            return View(tranList);

        }
        #endregion

        #region "Create a Currency Trade" - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult CreateCurrencyTrade()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult CreateCurrencyTrade(CurrencyTrade TR)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Invalid Input";
                TempData["MsgType"] = "warning";
                return View("CreateCurrencyTrade");
            }
            else
            {
                string findAmt = @"SELECT * FROM Stock WHERE ISO='{0}'";
                string combine1 = String.Format(findAmt, TR.QuoteCurrency.EscQuote());

                List<Stock> sList = DBUtl.GetList<Stock>(combine1);
                if (sList.Count == 1)
                {
                    if (sList[0].Amount >= TR.QuoteAmount)
                    {
                        string updateMoney = @"UPDATE Stock SET Amount={0} WHERE StockId={1}";
                        int res = DBUtl.ExecSQL(updateMoney, sList[0].Amount - TR.QuoteAmount, sList[0].StockId);
                        if (res == 1)
                        {
                            string sql = @"INSERT INTO CurrencyTrade(BaseCurrency, BaseAmount, QuoteCurrency, 
                            QuoteAmount, ExchangeRate, TransactionDate, DoneBy, Deleted) 
                            VALUES('{0}', {1}, '{2}', {3}, {4}, '{5:yyyy-MM-dd HH:mm:ss}', '{6}', 'false')";

                            string insert = String.Format(sql, TR.BaseCurrency.EscQuote(), TR.BaseAmount,
                                TR.QuoteCurrency.EscQuote(), TR.QuoteAmount, TR.ExchangeRate, DateTime.Now, User.Identity.Name.EscQuote());

                            if (DBUtl.ExecSQL(insert) == 1)
                            {
                                TempData["Message"] = "Currency Trade Successfully Added.";
                                TempData["MsgType"] = "success";
                                return RedirectToAction("CurrencyTradeIndex");
                            }
                            else
                            {
                                ViewData["Message"] = DBUtl.DB_Message;
                                ViewData["MsgType"] = "danger";
                                return View("CreateCurrencyTrade");
                            }
                        }
                        else
                        {
                            ViewData["Message"] = DBUtl.DB_Message;
                            ViewData["MsgType"] = "danger";
                            return View("CreateCurrencyTrade");
                        }
                    }
                    else
                    {
                        TempData["Message"] = "We have insufficient balance to go through with this exchange";
                        TempData["MsgType"] = "warning";
                        return View("CreateCurrencyTrade");
                    }
                }
                else
                {
                    TempData["Message"] = "Selected Stock Does not exist " + TR.QuoteCurrency;
                    TempData["MsgType"] = "warning";
                    return View("CreateCurrencyTrade");
                }
            }
        }
        #endregion

        #region "Edit Currency Trade" - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult CurrencyTradeEdit(int id)
        {
            string sql = @"SELECT * FROM CurrencyTrade WHERE TransactionId={0}";

            string select = String.Format(sql, id);
            List<CurrencyTrade> TRlist = DBUtl.GetList<CurrencyTrade>(select);
            if (TRlist.Count == 1)
            {
                CurrencyTrade TR = TRlist[0];
                return View(TR);
            }
            else
            {
                TempData["Message"] = "Currency Trade Record does not exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("CurrencyTradeIndex");
            }
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult CurrencyTradeEdit(CurrencyTrade TR)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Invalid Input";
                TempData["MsgType"] = "warning";
                return View("CurrencyTradeEdit", TR);
            }
            else
            {
                string sql = @"UPDATE CurrencyTrade  
                              SET BaseCurrency='{1}', BaseAmount={2}, QuoteCurrency='{3}',
                                  QuoteAmount={4}, ExchangeRate={5}, TransactionDate='{6:yyyy-MM-dd}',
                                  EditedBy='{7}', EditedDate='{8:yyyy-MM-dd}'
                              WHERE TransactionId={0}";
                string update = String.Format(sql, TR.TransactionId, TR.BaseCurrency.EscQuote(), TR.BaseAmount,
                    TR.QuoteCurrency.EscQuote(), TR.QuoteAmount, TR.ExchangeRate, TR.TransactionDate,
                    User.Identity.Name.EscQuote(), DateTime.Now);

                if (DBUtl.ExecSQL(update) == 1)
                {
                    TempData["Message"] = "Transaction Updated";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                }
                return RedirectToAction("CurrencyTradeIndex");
            }
        }
        #endregion

        #region "Soft Delete a Currency Trade" - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult SoftDeleteCurrencyTrade(int id)
        {
            string sql = @"SELECT * FROM CurrencyTrade 
                         WHERE TransactionId={0}";

            string select = String.Format(sql, id);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Transaction Record does not exist";
                TempData["MsgType"] = "warning";
            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format("UPDATE CurrencyTrade SET Deleted='True',DeletedBy='{1}' WHERE TransactionId={0}", id, User.Identity.Name.EscQuote()));
                if (res == 1)
                {
                    TempData["Message"] = "Transaction Record Deleted";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("CurrencyTradeIndex");
        }
        #endregion

        #region "Permanently Delete a Currency Trade" - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult PermanentDeleteCurrencyTrade(int id)
        {
            string sql = @"SELECT * FROM CurrencyTrade 
                         WHERE TransactionId={0}";

            string select = String.Format(sql, id);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Currency Trade Record does not exist";
                TempData["MsgType"] = "warning";
            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format("DELETE FROM CurrencyTrade WHERE TransactionId={0}", id));
                if (res == 1)
                {
                    TempData["Message"] = "Currency Trade Record Deleted Permanently";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("DeletedCurrencyTrades");
        }
        #endregion

        #region "Recover Deleted Currency Trade" - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult RecoverCurrencyTrade(int id)
        {
            string sql = @"SELECT * FROM CurrencyTrade 
                         WHERE TransactionId={0}";

            string select = String.Format(sql, id);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Currency Trade Record does not exist";
                TempData["MsgType"] = "warning";
            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format("UPDATE CurrencyTrade SET Deleted='False', Deletedby='null' WHERE TransactionId={0}", id));
                if (res == 1)
                {
                    TempData["Message"] = "Currency Trade Record Recovered";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("DeletedCurrencyTrades");
        }
        #endregion

        #region"Currency Trade Record Details" - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult CurrencyTradeDetails(int id)
        {
            string sql = @"SELECT * FROM CurrencyTrade WHERE TransactionId={0}";
            List<CurrencyTrade> TRlist = DBUtl.GetList<CurrencyTrade>(String.Format(sql, id));
            if (TRlist.Count > 0)
            {
                return View(TRlist[0]);
            }
            else
            {
                return NotFound();
            }
        }
        #endregion

    }
}
