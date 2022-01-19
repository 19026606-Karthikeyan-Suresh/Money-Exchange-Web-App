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
        public IActionResult ExchangeRates()
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

            List<ExchangeRates> curExList = DBUtl.GetList<ExchangeRates>("SELECT * FROM ExchangeRates ORDER BY Target_currency");

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

        #region Top5Currencies - Teng Yik
        public IActionResult Top5Currencies()
        {
            return View();
        }

        // [HttpPost]
        // public IActionResult Top5Currencies()
        //{
        /*if (!ModelState.IsValid)
        {
            ViewData["Message"] = "Invalid choice.";
            ViewData["MsgType"] = "warning";
        } 
        else
        {*/
        /*string sql = @"SELECT DISTINCT Source_currency AS [ISO], COUNT(DISTINCT Source_currency) AS [ISO count]
                       FROM Transactions 
                       WHERE MONTH(Transaction_date) = 1";*/
        /*AND Deleted = 'False' 
        ORDER BY 'ISO count' '{0}'";*/

        // string count = "";

        /*if (tp5c.BestOrWorst.Equals("Best"))
        {
            count = String.Format(sql, tp5c.Month, "DESC");
        }
        if (tp5c.BestOrWorst.Equals("Worst"))
        {
            count = String.Format(sql, tp5c.Month, "ASC");
        }*/

        // DataTable dt = DBUtl.GetTable(sql);



        //return RedirectToAction("DisplayResults", dt);
        // }
        //return View("Top5Currencies");
        // }
        #endregion

        public IActionResult DisplayResults()
        {
            /*if(dt.Columns.Count == 0)
            {
                return RedirectToAction("Top5Currencies");
            } 
            else
            {
                return View(dt);
            }*/

            string sql = @"SELECT COUNT(Source_currency) AS 'ISO'
                           FROM Transactions
                           ORDER BY Source_currency DESC ";
                              // WHERE MONTH(Transaction_date) = 1";

            DataTable dt = DBUtl.GetTable(sql);

            return View(dt);






        }
    }
}