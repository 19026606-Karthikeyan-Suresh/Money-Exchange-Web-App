using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;

namespace MoneyExchangeWebApp.Controllers
{
    public class ReportController : Controller
    {
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

            string sql = @"SELECT TOP(5) BaseCurrency AS 'ISO', COUNT(*) AS 'No. of Trades' FROM ConvTransactions 
                           WHERE MONTH(TransactionDate) = {0}
                           GROUP BY BaseCurrency ORDER BY 'No. of Trades' DESC ";

            string select = String.Format(sql, apple);


            DataTable dt = DBUtl.GetTable(select);

            return View(dt);
        }

        public IActionResult DisplayWorstCurrencies()  //need to use TOP(5) as well
        {
            IFormCollection form = HttpContext.Request.Form;

            string month = form["Month"].ToString().Trim();

            int apple = Int32.Parse(month);

            string sql = @"SELECT TOP(5) BaseCurrency AS 'ISO', COUNT(*) AS 'No. of Trades' FROM ConvTransactions 
                           WHERE MONTH(TransactionDate) = {0}
                           GROUP BY BaseCurrency ORDER BY 'No. of Trades' ASC ";

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

            string sql = @"SELECT TOP(5) TransactionDate AS 'Day', COUNT(TransactionId) AS 'No. of Trades'
                           FROM ConvTransactions
                          WHERE MONTH(TransactionDate) = {0}
                          GROUP BY TransactionDate ORDER BY 'No. of Trades' DESC";


            string select = String.Format(sql, apple);


            DataTable dt = DBUtl.GetTable(select);

            return View(dt);
        }

        public IActionResult DisplayWorstDays()
        {
            IFormCollection form = HttpContext.Request.Form;

            string month = form["Month"].ToString().Trim();

            int apple = Int32.Parse(month);

            string sql = @"SELECT TOP(5) TransactionDate AS 'Day', COUNT(TransactionId) AS 'No. of Trades'
                           FROM ConvTransactions
                          WHERE MONTH(TransactionDate) = {0}
                          GROUP BY TransactionDate ORDER BY 'No. of Trades' ASC";


            string select = String.Format(sql, apple);


            DataTable dt = DBUtl.GetTable(select);

            return View(dt);
        }



    }
}
