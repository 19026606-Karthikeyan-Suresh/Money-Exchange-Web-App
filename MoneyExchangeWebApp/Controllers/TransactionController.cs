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
    public class TransactionController : Controller
    {
        #region "View All Transactions" - Karthik
        [HttpGet]
        [Authorize(Roles = "admin")]
        public IActionResult GetAllTransactions()
        {
            string sql;
            sql = @"SELECT * FROM Transactions WHERE Deleted='False'";
            var TRlist = DBUtl.GetList<Transaction>(sql);
            return Json(new { data = TRlist });
        }

        [Authorize(Roles = "admin")]
        public IActionResult TransactionIndex()
        {
            return View();
        }
        #endregion

        #region "View Deleted Transactions" - Karthik
        [Authorize(Roles ="admin")]
        public IActionResult DeletedTransactions()
        {
            List<Transaction> tranList = DBUtl.GetList<Transaction>("SELECT * FROM Transactions WHERE Deleted='True' ORDER BY TransactionDate DESC");
            return View(tranList);

        }
        #endregion

        #region "Create Transaction" - Karthik
        [Authorize(Roles ="admin")]
        public IActionResult CreateTransaction()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateTransaction(Transaction TR)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("CreateTransaction");
            }
            else
            {
                string user = User.Identity.Name;
                string sql = @"INSERT INTO Transactions(BaseCurrency, BaseAmount, QuoteCurrency, 
                QuoteAmount, ExchangeRate, TransactionDate, DoneBy, EditedBy, EditedDate, Deleted, DeletedBy, DeletedDate) 
                VALUES('{0}', {1}, '{2}', {3}, {4}, '{5:yyyy-MM-dd}', '{6}', '{7}', '{8:yyyy-MM-dd}', {9}, '{10}', '{11:yyyy-MM-dd}')";

                string insert = String.Format(sql, TR.BaseCurrency.EscQuote(), TR.BaseAmount,
                    TR.QuoteCurrency.EscQuote(), TR.QuoteAmount, TR.ExchangeRate, TR.TransactionDate, TR.DoneBy, null, null, 0, null, null);

                if (DBUtl.ExecSQL(insert) == 1)
                {
                    TempData["Message"] = "Transaction Successfully Added.";
                    TempData["MsgType"] = "success";
                    return RedirectToAction("TransactionIndex");
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                    return View("CreateTransaction");
                }
            }
        }
        #endregion

        #region "Edit Transaction" - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult TransactionEdit(int id)
        {
            string sql = @"SELECT * FROM Transactions WHERE TransactionId={0}";

            string select = String.Format(sql, id);
            List<Transaction> TRlist = DBUtl.GetList<Transaction>(select);
            if (TRlist.Count == 1)
            {
                Transaction TR = TRlist[0];
                return View(TR);
            }
            else
            {
                TempData["Message"] = "Transaction Record does not exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("TransactionIndex");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult TransactionEdit(Transaction TR)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "danger";
                return View("TransactionEdit", TR);
            }
            else
            {
                string sql = @"UPDATE Transactions  
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
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("TransactionIndex");
            }
        }
        #endregion

        #region "Soft Delete Transaction" - Karthik
        [Authorize(Roles ="admin")]
        public IActionResult SoftDelete(int id)
        {
            string sql = @"SELECT * FROM Transactions 
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
                int res = DBUtl.ExecSQL(String.Format("UPDATE Transactions SET Deleted='True',DeletedBy='{1}' WHERE TransactionId={0}", id, User.Identity.Name.EscQuote()));
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
            return RedirectToAction("TransactionIndex");
        }
        #endregion

        #region "Permanently Delete Transaction" - Karthik
        [Authorize]
        public IActionResult PermanentDelete(int id)
        {
            string sql = @"SELECT * FROM Transactions 
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
                int res = DBUtl.ExecSQL(String.Format("DELETE FROM Transactions WHERE TransactionId={0}", id));
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
            return RedirectToAction("DeletedTransactions");
        }
        #endregion

        #region "Recover Deleted Transaction" - Karthik
        [Authorize]
        public IActionResult RecoverTransaction(int id)
        {
            string sql = @"SELECT * FROM Transactions 
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
                int res = DBUtl.ExecSQL(String.Format("UPDATE Transactions SET Deleted='False', Deletedby='null' WHERE TransactionId={0}", id));
                if (res == 1)
                {
                    TempData["Message"] = "Transaction Record Recovered";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("DeletedTransactions");
        }
        #endregion

        #region"Transaction Details" - Karthik
        public IActionResult TransactionDetails(int id)
        {
            string sql = @"SELECT * FROM Transactions WHERE TransactionId={0}";
            List<Transaction> TRlist = DBUtl.GetList<Transaction>(String.Format(sql, id));
            if(TRlist.Count > 0)
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
