using MoneyExchangeWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System;

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

        /*        #region Error Message
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #endregion*/

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

        public IActionResult ExchangeRates()
        {
            return View();
        }
    }
}