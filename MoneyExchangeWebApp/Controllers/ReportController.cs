using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Data;

namespace MoneyExchangeWebApp.Controllers
{
    public class ReportController : Controller
    {

        //Top or Worst 5 Currencies
        public IActionResult TradesDashboard()
        {
            return View();
        }

        public IActionResult DisplayCurrencies()
        {

            IFormCollection form = HttpContext.Request.Form;

            string bestorworst = form["BestOrWorst"].ToString().Trim();
            string month = form["Month"].ToString().Trim();
            int apple = Int32.Parse(month);

            string sql = "";
            string select = "";

            if (bestorworst.Equals("Best"))
            {
                sql = @"SELECT TOP(5) BaseCurrency AS 'ISO', COUNT(*) AS 'No. of Trades' FROM ConvTransactions 
                           WHERE MONTH(TransactionDate) = {0}
                           GROUP BY BaseCurrency ORDER BY 'No. of Trades' DESC ";

                TempData["BestorWorst"] = "best";
                select = String.Format(sql, apple);

            }

            if (bestorworst.Equals("Worst"))
            {
                sql = @"SELECT TOP(5) BaseCurrency AS 'ISO', COUNT(*) AS 'No. of Trades' FROM ConvTransactions 
                           WHERE MONTH(TransactionDate) = {0}
                           GROUP BY BaseCurrency ORDER BY 'No. of Trades' ASC ";

                TempData["BestorWorst"] = "worst";
                select = String.Format(sql, apple);
            }

            DataTable dt = DBUtl.GetTable(select);

            return View(dt);
        }

        //Top or Worst 5 Trading Days by Month

        public IActionResult DaysDashboard()
        {
            return View();
        }

        public IActionResult DisplayDays()
        {

            IFormCollection form = HttpContext.Request.Form;

            string bestorworst = form["BestOrWorst"].ToString().Trim();
            string month = form["Month"].ToString().Trim();
            int apple = Int32.Parse(month);

            string sql = "";
            string select = "";

            if (bestorworst.Equals("Best"))
            {
                sql = @"SELECT TOP(5) TransactionDate AS 'Day', COUNT(TransactionId) AS 'No. of Trades'
                           FROM ConvTransactions
                          WHERE MONTH(TransactionDate) = {0}
                          GROUP BY TransactionDate ORDER BY 'No. of Trades' DESC";

                TempData["BestorWorst"] = "best";
                select = String.Format(sql, apple);

            }

            if (bestorworst.Equals("Worst"))
            {
                sql = @"SELECT TOP(5) TransactionDate AS 'Day', COUNT(TransactionId) AS 'No. of Trades'
                           FROM ConvTransactions
                          WHERE MONTH(TransactionDate) = {0}
                          GROUP BY TransactionDate ORDER BY 'No. of Trades' ASC";

                TempData["BestorWorst"] = "worst";
                select = String.Format(sql, apple);
            }

            DataTable dt = DBUtl.GetTable(select);

            return View(dt);
        }
    }
}
