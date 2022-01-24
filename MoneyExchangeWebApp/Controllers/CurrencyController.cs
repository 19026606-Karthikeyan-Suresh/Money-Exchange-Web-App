using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyExchangeWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

//using Newtonsoft.Json;

namespace MoneyExchangeWebApp.Controllers
{
    public class CurrencyController : Controller
    {
        #region "Display ExchangeRates" - Jasper
        private string apiWebsite = "https://freecurrencyapi.net/api/v2/latest?apikey=d7ba8d40-5e88-11ec-b4e7-e7f7a5d589f5&base_currency=SGD";
        public IActionResult GetAllExchangeRates()
        {
            dynamic data = WebUtl.CallWebApi(apiWebsite);
            dynamic result = new
            {
                query = data.query,
                data = data.data
            };
            var curList = result.data.ToString().Replace("{", "").Replace("}", "");
            string[] list = curList.Split(",");

            DBUtl.ExecSQL("DELETE FROM ExchangeRates");

            for (int i = 0; i < list.Length; i++)
            {
                string[] currNRate = list[i].Split(":");

                //DO NOT DELETE THESE COMMENTED CODES FIRST
                //    List<CurrencyExchange> curExList = DBUtl.GetList<CurrencyExchange>($"SELECT * FROM ExchangeRates WHERE Target_currency = '{currNRate[0].Replace("\"", "").Trim()}'");
                //    ViewData["Check"] = $"SELECT * FROM ExchangeRates WHERE Target_currency = '{currNRate[0].Replace("\"","").Trim()}'";

                //    if (curExList.Count == 0)
                //    {
                //        string sql = @"INSERT INTO ExchangeRates VALUES ('SGD', '{0}', {1})";
                //        if(DBUtl.ExecSQL(sql, currNRate[0], currNRate[1]) == 1)
                //        {

                //        }
                //        else                       
                //            ViewData["DatabaseUpdateError"] = DBUtl.DB_Message;               
                //}
                //    else
                //    {
                //        string sql = @"UPDATE ExchangeRates SET Exchange_rate = {0} WHERE Target_currency = '{1}'";
                //        int success = DBUtl.ExecSQL(sql, currNRate[1], currNRate[0]);
                //        if (success == 0)
                //            ViewData["DatabaseUpdateError"] = "There was an error updating the database. Please contact the Administrator if you see this message";                  
                //    }
                string sql = @"INSERT INTO ExchangeRates VALUES ('SGD', '{0}', {1})";
                if (DBUtl.ExecSQL(sql, currNRate[0].Replace("\"", "").Trim(), Convert.ToDouble(currNRate[1])) != 1)
                {
                    ViewData["DatabaseUpdateError"] = DBUtl.DB_Message;
                }
                ViewData["Check"] = String.Format(sql, currNRate[0].Replace("\"", "").Trim(), Convert.ToDouble(currNRate[1]));

            }

            var curExList = DBUtl.GetList<ExchangeRates>("SELECT * FROM ExchangeRates ORDER BY QuoteCurrency");

            return Json(new { data = curExList });

        }
        #endregion

        #region
        public IActionResult ExchangeRates()
        {
            return View();
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

        /*#region Top5Currencies - Teng Yik
        public IActionResult Top5Currencies()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Top5Currencies(Top5Currency tp5c)   //Top 5 currency traded by month
        {
        if (!ModelState.IsValid)
        {
            ViewData["Message"] = "Invalid choice.";
            ViewData["MsgType"] = "warning";
        } 
        else
        {
        string sql = @"SELECT DISTINCT Source_currency AS [ISO], COUNT(DISTINCT Source_currency) AS [ISO count]
                       FROM Transactions 
                       WHERE MONTH(Transaction_date) = 1
                       ND Deleted = 'False'
                        ORDER BY 'ISO count' '{0}'";  

        string count = "";

        if (tp5c.BestOrWorst.Equals("Best"))
        {
            count = String.Format(sql, tp5c.Month, "DESC");
        }
        if (tp5c.BestOrWorst.Equals("Worst"))
        {
            count = String.Format(sql, tp5c.Month, "ASC");
        }

        DataTable dt = DBUtl.GetTable(sql);



        return RedirectToAction("DisplayResults", dt);
         }
        return View("Top5Currencies");
         }
        #endregion*/

        // Top or Worst 5 Currencies Traded by Month
        public IActionResult TradesDashboard()
        {
            return View();
        }

        public IActionResult RedirectTrades()
        {
            IFormCollection form = HttpContext.Request.Form;

            string bestorworst = form["BestOrWorst"].ToString().Trim();

            if (bestorworst.Equals("Best"))
            {
                return RedirectToAction("Top5Currencies");
            }

            if (bestorworst.Equals("Worst"))
            {
                return RedirectToAction("Worst5Currencies");
            }

            return View();
        }

        public IActionResult Top5Currencies()
        {
            return View();
        }
        public IActionResult Worst5Currencies()
        {
            return View();
        }
        public IActionResult DisplayTopCurrencies()   //need to use TOP(5) as well
        {

                IFormCollection form = HttpContext.Request.Form;

                string month = form["Month"].ToString().Trim();

                int apple = Int32.Parse(month);

                string sql = @"SELECT TOP(5) Source_currency AS 'ISO', COUNT(*) AS 'No. of Trades' FROM Transactions 
                           WHERE MONTH(Transaction_date) = {0}
                           GROUP BY Source_currency ORDER BY 'No. of Trades' DESC ";

                string select = String.Format(sql, apple);


                DataTable dt = DBUtl.GetTable(select);

                return View(dt);
        }

        public IActionResult DisplayWorstCurrencies()  //need to use TOP(5) as well
        {
            IFormCollection form = HttpContext.Request.Form;

            string month = form["Month"].ToString().Trim();

            int apple = Int32.Parse(month);

            string sql = @"SELECT TOP(5) Source_currency AS 'ISO', COUNT(*) AS 'No. of Trades' FROM Transactions 
                           WHERE MONTH(Transaction_date) = {0}
                           GROUP BY Source_currency ORDER BY 'No. of Trades' ASC ";

            string select = String.Format(sql, apple);


            DataTable dt = DBUtl.GetTable(select);

            return View(dt);
        }


        //Top or Worst 5 Trading Days by Month

        public IActionResult DaysDashboard()
        {
            return View();
        }

        public IActionResult RedirectDays()
        {
            IFormCollection form = HttpContext.Request.Form;

            string bestorworst = form["BestOrWorst"].ToString().Trim();

            if (bestorworst.Equals("Best"))
            {
                return RedirectToAction("TopDays");
            }

            if (bestorworst.Equals("Worst"))
            {
                return RedirectToAction("WorstDays");
            }

            return View();
        }

        public IActionResult TopDays()
        {
            return View();
        }

        public IActionResult WorstDays()
        {
            return View();
        }

        public IActionResult DisplayTopDays()
        {
            IFormCollection form = HttpContext.Request.Form;

            string month = form["Month"].ToString().Trim();

            int apple = Int32.Parse(month);

            string sql = @"SELECT TOP(5) Transaction_date AS 'Day', COUNT(Transaction_id) AS 'No. of Trades'
                           FROM Transactions
                          WHERE MONTH(Transaction_date) = {0}
                          GROUP BY Transaction_date ORDER BY 'No. of Trades' DESC";


            string select = String.Format(sql, apple);


            DataTable dt = DBUtl.GetTable(select);

            return View(dt);
        }

        public IActionResult DisplayWorstDays()
        {
            IFormCollection form = HttpContext.Request.Form;

            string month = form["Month"].ToString().Trim();

            int apple = Int32.Parse(month);

            string sql = @"SELECT TOP(5) Transaction_date AS 'Day', COUNT(Transaction_id) AS 'No. of Trades'
                           FROM Transactions
                          WHERE MONTH(Transaction_date) = {0}
                          GROUP BY Transaction_date ORDER BY 'No. of Trades' ASC";


            string select = String.Format(sql, apple);


            DataTable dt = DBUtl.GetTable(select);

            return View(dt);
        }

    }
}