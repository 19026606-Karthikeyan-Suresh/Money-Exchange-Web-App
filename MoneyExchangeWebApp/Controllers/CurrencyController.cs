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

        
    }
}