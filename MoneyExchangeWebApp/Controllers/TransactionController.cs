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

        #region "Transaction View ALL: - Karthik
        public IActionResult AllTransactions()
        {
            List<Transaction> tranList = DBUtl.GetList<Transaction>("SELECT * FROM Transactions ORDER BY Transaction_date DESC");
            return View(tranList);

        }
        #endregion

        #region "Transaction Create - Karthik"
        public IActionResult CreateTransaction()
        {
            return View();
        }
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
                string sql = @"INSERT INTO Transactions(Source_currency, Source_amount, Converted_currency, 
Converted_amount, exchange_rate, Transaction_date) VALUES('{0}', {1}, '{2}', {3}, {4}, '{5:yyyy-MM-dd}')";

                string insert = String.Format(sql, TR.Source_currency.EscQuote(),TR.Source_amount, 
                    TR.Converted_currency.EscQuote(), TR.Converted_amount, TR.Exchange_rate, TR.Transaction_date);

                if (DBUtl.ExecSQL(insert) == 1)
                {
                    TempData["Message"] = "Transaction Successfully Added.";
                    TempData["MsgType"] = "success";
                    return RedirectToAction("AllTransactions");
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

        #region "Transaction Delete - Karthik"
        public IActionResult Delete(int id)
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
                    TempData["Message"] = "Transaction Record Deleted";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("AllTransactions");
        }
        #endregion
    }
}
