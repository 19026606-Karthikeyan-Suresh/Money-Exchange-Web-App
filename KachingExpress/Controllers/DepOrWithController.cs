using KachingExpress.Models;
using Microsoft.AspNetCore.Mvc;

namespace KachingExpress.Controllers
{
    public class DepOrWithController : Controller
    {
        #region List all transactions for specific currency - Karthik
        public IActionResult DOWTransactions(int id)
        {
            string sql = @"SELECT * FROM DepWithTransactions WHERE StockId={0}";
            List<DepWithTransactions> DWTlist = DBUtl.GetList<DepWithTransactions>(sql, id);
            return View(DWTlist);
        }
        #endregion
    }
}
