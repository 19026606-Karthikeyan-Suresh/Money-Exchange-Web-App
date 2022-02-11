﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MoneyExchangeWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;
using System.Data;

namespace MoneyExchangeWebApp.Controllers
{
    public class ConvTransactionController : Controller
    {
        #region "View All ConvTransactions" - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult ConvTransactionIndex()
        {
            string sql = @"SELECT * FROM ConvTransactions WHERE Deleted='False'";
            List<ConvTransaction> CTRlist = DBUtl.GetList<ConvTransaction>(sql);
            return View("ConvTransactionIndex", CTRlist);
        }
        #endregion

        #region "View Deleted Transactions" - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult DeletedConvTransactions()
        {
            List<ConvTransaction> tranList = DBUtl.GetList<ConvTransaction>("SELECT * FROM ConvTransactions WHERE Deleted='True' ORDER BY TransactionDate DESC");
            return View(tranList);

        }
        #endregion

        #region "Create Transaction" - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult CreateConvTransaction()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult CreateConvTransaction(ConvTransaction TR)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Invalid Input";
                TempData["MsgType"] = "warning";
                return View("CreateConvTransaction");
            }
            else
            {
                string findAmt = @"SELECT * FROM Stock WHERE ISO='{0}'";
                string combine1 = String.Format(findAmt, TR.QuoteCurrency.EscQuote());

                List<Stock> sList = DBUtl.GetList<Stock>(combine1);
                if(sList.Count == 1)
                {
                    if(sList[0].Amount >= TR.QuoteAmount)
                    {
                        string updateMoney = @"UPDATE Stock SET Amount={0} WHERE StockId={1}";
                        int res = DBUtl.ExecSQL(updateMoney, sList[0].Amount - TR.QuoteAmount, sList[0].StockId);
                        if(res == 1)
                        {
                            string sql = @"INSERT INTO ConvTransactions(BaseCurrency, BaseAmount, QuoteCurrency, 
                            QuoteAmount, ExchangeRate, TransactionDate, DoneBy, Deleted) 
                            VALUES('{0}', {1}, '{2}', {3}, {4}, '{5:yyyy-MM-dd HH:mm:ss}', '{6}', 'false')";

                            string insert = String.Format(sql, TR.BaseCurrency.EscQuote(), TR.BaseAmount,
                                TR.QuoteCurrency.EscQuote(), TR.QuoteAmount, TR.ExchangeRate, DateTime.Now, User.Identity.Name.EscQuote());

                            if (DBUtl.ExecSQL(insert) == 1)
                            {
                                TempData["Message"] = "Currency Trade Successfully Added.";
                                TempData["MsgType"] = "success";
                                return RedirectToAction("ConvTransactionIndex");
                            }
                            else
                            {
                                ViewData["Message"] = DBUtl.DB_Message;
                                ViewData["MsgType"] = "danger";
                                return View("CreateConvTransaction");
                            }
                        }
                        else
                        {
                            ViewData["Message"] = DBUtl.DB_Message;
                            ViewData["MsgType"] = "danger";
                            return View("CreateConvTransaction");
                        }
                    }
                    else
                    {
                        ViewData["Message"] = "We have insufficient balance to go through with this exchange";
                        ViewData["MsgType"] = "warning";
                        return View("CreateConvTransaction");
                    }
                }
                else
                {
                    ViewData["Message"] = "Selected Stock Does not exist " + TR.QuoteCurrency;
                    ViewData["MsgType"] = "warning";
                    return View("CreateConvTransaction");
                }
            }
        }
        #endregion

        #region "Edit Transaction" - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult ConvTransactionEdit(int id)
        {
            string sql = @"SELECT * FROM ConvTransactions WHERE TransactionId={0}";

            string select = String.Format(sql, id);
            List<ConvTransaction> TRlist = DBUtl.GetList<ConvTransaction>(select);
            if (TRlist.Count == 1)
            {
                ConvTransaction TR = TRlist[0];
                return View(TR);
            }
            else
            {
                TempData["error"] = "Transaction Record does not exist";
                return RedirectToAction("ConvTransactionIndex");
            }
        }

        [Authorize(Roles ="admin")]
        [HttpPost]
        public IActionResult ConvTransactionEdit(ConvTransaction TR)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Invalid Input";
                return View("ConvTransactionEdit", TR);
            }
            else
            {
                string sql = @"UPDATE ConvTransactions  
                              SET BaseCurrency='{1}', BaseAmount={2}, QuoteCurrency='{3}',
                                  QuoteAmount={4}, ExchangeRate={5}, TransactionDate='{6:yyyy-MM-dd}',
                                  EditedBy='{7}', EditedDate='{8:yyyy-MM-dd}'
                              WHERE TransactionId={0}";
                string update = String.Format(sql, TR.TransactionId, TR.BaseCurrency.EscQuote(), TR.BaseAmount,
                    TR.QuoteCurrency.EscQuote(), TR.QuoteAmount, TR.ExchangeRate, TR.TransactionDate,
                    User.Identity.Name.EscQuote(), DateTime.Now);

                if (DBUtl.ExecSQL(update) == 1)
                {
                    TempData["success"] = "Transaction Updated";
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                }
                return RedirectToAction("ConvTransactionIndex");
            }
        }
        #endregion

        #region "Soft Delete Transaction" - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult SoftDeleteConvTransaction(int id)
        {
            string sql = @"SELECT * FROM ConvTransactions 
                         WHERE TransactionId={0}";

            string select = String.Format(sql, id);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Transaction Record does not exist";
            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format("UPDATE ConvTransactions SET Deleted='True',DeletedBy='{1}', WHERE TransactionId={0}", id, User.Identity.Name.EscQuote()));
                if (res == 1)
                {
                    TempData["success"] = "Transaction Record Deleted";
                }
                else
                {
                    TempData["error"] = DBUtl.DB_Message;
                }
            }
            return RedirectToAction("ConvTransactionIndex");
        }
        #endregion

        #region "Permanently Delete Transaction" - Karthik
        [Authorize(Roles ="admin")]
        public IActionResult PermanentDeleteConvTransaction(int id)
        {
            string sql = @"SELECT * FROM ConvTransactions 
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
                int res = DBUtl.ExecSQL(String.Format("DELETE FROM ConvTransactions WHERE TransactionId={0}", id));
                if (res == 1)
                {
                    TempData["Message"] = "Transaction Record Deleted Permanently";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("DeletedConvTransactions");
        }
        #endregion

        #region "Recover Deleted Transaction" - Karthik
        [Authorize(Roles ="admin")]
        public IActionResult RecoverTransaction(int id)
        {
            string sql = @"SELECT * FROM ConvTransactions 
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
                int res = DBUtl.ExecSQL(String.Format("UPDATE ConvTransactions SET Deleted='False', Deletedby='null' WHERE TransactionId={0}", id));
                if (res == 1)
                {
                    TempData["Message"] = "Conversion Transaction Record Recovered";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("DeletedConvTransactions");
        }
        #endregion

        #region"Conversion Transaction Details" - Karthik
        [Authorize(Roles ="admin")]
        public IActionResult ConvTransactionDetails(int id)
        {
            string sql = @"SELECT * FROM ConvTransactions WHERE TransactionId={0}";
            List<ConvTransaction> TRlist = DBUtl.GetList<ConvTransaction>(String.Format(sql, id));
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
