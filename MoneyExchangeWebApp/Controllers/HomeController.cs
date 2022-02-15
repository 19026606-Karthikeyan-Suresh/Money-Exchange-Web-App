using MoneyExchangeWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;
using System.Collections.Generic;

namespace MoneyExchangeWebApp.Controllers
{
    public class HomeController : Controller
    {
        #region Home Page - Karthik
        public IActionResult Index()
        {
            return View();
        }
        #endregion

        #region "Display ExchangeRates" - Jasper
        private string apiWebsite = "https://freecurrencyapi.net/api/v2/latest?apikey=364830c0-5907-11ec-ad52-3b41478b07db&base_currency=SGD";
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

                string sql = @"INSERT INTO ExchangeRates VALUES ('SGD', '{0}', {1})";
                if (DBUtl.ExecSQL(sql, currNRate[0].Replace("\"", "").Trim(), Convert.ToDouble(currNRate[1])) != 1)
                {
                    ViewData["DatabaseUpdateError"] = DBUtl.DB_Message;
                }
                ViewData["Check"] = String.Format(sql, currNRate[0].Replace("\"", "").Trim(), Convert.ToDouble(currNRate[1]));

            }

            List<ExchangeRates> curExList = DBUtl.GetList<ExchangeRates>("SELECT * FROM ExchangeRates ORDER BY QuoteCurrency");



            return View(curExList);

        }
        #endregion
    }
}