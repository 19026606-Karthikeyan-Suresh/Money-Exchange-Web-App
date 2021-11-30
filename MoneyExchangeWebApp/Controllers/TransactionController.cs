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
        [Authorize]
        public IActionResult TransactionIndex()
        {
            List<Transaction> tranList = DBUtl.GetList<Transaction>("SELECT * FROM Transactions WHERE Deleted='False' ORDER BY Transaction_date DESC");
            return View(tranList);

        }
        #endregion

        #region "View Deleted Transactions" - Karthik
        [Authorize]
        public IActionResult DeletedTransactions()
        {
            List<Transaction> tranList = DBUtl.GetList<Transaction>("SELECT * FROM Transactions WHERE Deleted='True' ORDER BY Transaction_date DESC");
            return View(tranList);

        }
        #endregion

        #region "Create Transaction" - Karthik
        [Authorize]
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
                string sql = @"INSERT INTO Transactions(Source_currency, Source_amount, Converted_currency, 
                Converted_amount, Exchange_rate, Transaction_date, Created_by, Deleted, Deleted_by) 
                VALUES('{0}', {1}, '{2}', {3}, {4}, '{5:yyyy-MM-dd}', '{6}', {7}, '{8}')";

                string insert = String.Format(sql, TR.Source_currency.EscQuote(), TR.Source_amount,
                    TR.Converted_currency.EscQuote(), TR.Converted_amount, TR.Exchange_rate, TR.Transaction_date, user, 0, null);

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
        [Authorize]
        public IActionResult TransactionEdit(int id)
        {
            string sql = @"SELECT * FROM Transactions WHERE Transaction_id={0}";

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
                              SET Source_currency='{1}', Source_amount={2}, Converted_currency='{3}',
                                  Converted_amount={4}, Exchange_rate={5}, Transaction_date='{6:yyyy-MM-dd}'
                            WHERE Transaction_id={0}";
                string update = String.Format(sql, TR.Transaction_id, TR.Source_currency.EscQuote(), TR.Source_amount,
                    TR.Converted_currency.EscQuote(), TR.Converted_amount, TR.Exchange_rate, TR.Transaction_date);

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
        [Authorize]
        public IActionResult SoftDelete(int id)
        {
            string sql = @"SELECT * FROM Transactions 
                         WHERE Transaction_id={0}";

            string select = String.Format(sql, id);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Transaction Record does not exist";
                TempData["MsgType"] = "warning";
            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format("UPDATE Transactions SET Deleted='True',Deleted_by='{1}' WHERE Transaction_id={0}", id, User.Identity.Name.EscQuote()));
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
                         WHERE Transaction_id={0}";

            string select = String.Format(sql, id);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Transaction Record does not exist";
                TempData["MsgType"] = "warning";
            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format("DELETE FROM Transactions WHERE Transaction_id={0}", id));
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
                         WHERE Transaction_id={0}";

            string select = String.Format(sql, id);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Transaction Record does not exist";
                TempData["MsgType"] = "warning";
            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format("UPDATE Transactions SET Deleted='False', Deleted_by='null' WHERE Transaction_id={0}", id));
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

    }
}
