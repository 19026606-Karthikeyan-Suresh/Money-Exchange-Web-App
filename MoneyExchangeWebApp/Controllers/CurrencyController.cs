using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyExchangeWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace MoneyExchangeWebApp.Controllers
{
    public class CurrencyController : Controller
    {
        #region "Display ExchangeRates" - Jasper
        public IActionResult ExchangeRates()
        {
            List<CurrencyExchange> curExList = DBUtl.GetList<CurrencyExchange>("SELECT * FROM ExchangeRates");
            return View(curExList);

        }
        #endregion

        #region "View Currencies" - Kaiwen
        [Authorize]
        public IActionResult CurrencyIndex()
        {
            List<Currency> curList = DBUtl.GetList<Currency>("SELECT * FROM Currency WHERE Deleted='False'");
            return View(curList);
        }
        #endregion

        #region "View Deleted Currencies" - Kaiwen
        [Authorize]
        public IActionResult DeletedCurrencies()
        {
            List<Currency> curList = DBUtl.GetList<Currency>("SELECT * FROM Currency WHERE Deleted='True'");
            return View(curList);
        }
        #endregion

        #region "Create a Currency" - Kaiwen
        [Authorize]
        public IActionResult CurrencyCreate()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult CurrencyCreate(Currency C)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("CurrencyCreate");
            }
            else
            {
                string user = User.Identity.Name;
                string sql = @"INSERT INTO Currency(Currency_name, Country, Average_rate, 
                Created_by, Created_date, Deleted, Deleted_by) 
                VALUES('{0}', '{1}', {2}, '{3}', '{4:yyyy-MM-dd}', {5}, '{6}')";

                string insert = String.Format(sql, C.Currency_name.EscQuote(), C.Country.EscQuote(),
                    C.Average_rate, user, C.Created_date ,0, null);

                if (DBUtl.ExecSQL(insert) == 1)
                {
                    TempData["Message"] = "Currency Successfully Added.";
                    TempData["MsgType"] = "success";
                    return RedirectToAction("CurrencyIndex");
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                    return View("CurrencyCreate");
                }
            }
        }
        #endregion

        #region "Edit Currency" - Kaiwen
        [Authorize]
        public IActionResult CurrencyEdit(int id)
        {
            string sql = @"SELECT * FROM Currency WHERE Currency_id={0}";

            string select = String.Format(sql, id);
            List<Currency> Clist = DBUtl.GetList<Currency>(select);
            if (Clist.Count == 1)
            {
                Currency C = Clist[0];
                return View(C);
            }
            else
            {
                TempData["Message"] = "Currency does not exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("CurrencyIndex");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult CurrencyEdit(Currency C)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "danger";
                return View("CurrencyEdit", C);
            }
            else
            {
                string sql = @"UPDATE Currency SET Currency_name='{1}', Country='{2}', Average_rate={3} WHERE Currency_id={0}";
                string update = String.Format(sql, C.Currency_id,C.Currency_name.EscQuote(), C.Country.EscQuote(), C.Average_rate);

                if (DBUtl.ExecSQL(update) == 1)
                {
                    TempData["Message"] = "Currency Updated";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("CurrencyIndex");
            }
        }
        #endregion

        #region "Soft Delete Currency" - Kaiwen
        [Authorize]
        public IActionResult SoftDelete(int id)
        {
            string sql = @"SELECT * FROM Currency 
                         WHERE Currency_id={0}";

            string select = String.Format(sql, id);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Currency does not exist";
                TempData["MsgType"] = "warning";
            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format("UPDATE Currency SET Deleted='True',Deleted_by='{1}' WHERE Currency_id={0}", id, User.Identity.Name.EscQuote()));
                if (res == 1)
                {
                    TempData["Message"] = "Currency Deleted";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("CurrencyIndex");
        }
        #endregion

        #region "Permanent Delete Currency" - Kaiwen
        [Authorize]
        public IActionResult PermanentDelete(int id)
        {
            string sql = @"SELECT * FROM Currency 
                         WHERE Currency_id={0}";

            string select = String.Format(sql, id);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Currency Record does not exist";
                TempData["MsgType"] = "warning";
            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format("DELETE FROM Currency WHERE Currency_id={0}", id));
                if (res == 1)
                {
                    TempData["Message"] = "Currency Deleted Permanently";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("DeletedCurrencies");
        }
        #endregion

        #region "Recover Deleted Currency" - Kaiwen
        [Authorize]
        public IActionResult RecoverCurrency(int id)
        {
            string sql = @"SELECT * FROM Currency 
                         WHERE Currency_id={0}";

            string select = String.Format(sql, id);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Currency does not exist";
                TempData["MsgType"] = "warning";
            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format("UPDATE Currency SET Deleted='False', Deleted_by='null' WHERE Currency_id={0}", id));
                if (res == 1)
                {
                    TempData["Message"] = "Currency Recovered";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("DeletedCurrencies");
        }
        #endregion
    }
}