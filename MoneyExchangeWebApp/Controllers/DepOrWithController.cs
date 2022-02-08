using MoneyExchangeWebApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Claims;

namespace MoneyExchangeWebApp.Controllers
{
    public class DepOrWithController : Controller
    {
        #region List all transactions for specific currency - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult DOWTransactions(int id)
        {
            string sql = @"SELECT * FROM DepWithTransactions WHERE StockId={0}";
            List<DepWithTransactions> DWTlist = DBUtl.GetList<DepWithTransactions>(sql, id);
            return View(DWTlist);
        }
        #endregion

        #region List all DepOrWith Transactions - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult DepOrWithIndex()
        {
            List<DepWithTransactions> dwl = DBUtl.GetList<DepWithTransactions>(@"SELECT * FROM DepWithTransactions WHERE Deleted=0");
            return View(dwl);
        }
        #endregion

        #region List all Deleted DepOrWith Transactions - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult DeletedDepOrWithIndex()
        {
            List<DepWithTransactions> dwl = DBUtl.GetList<DepWithTransactions>(@"SELECT * FROM DepWithTransactions WHERE Deleted=1");
            return View(dwl);
        }
        #endregion

        #region Soft Delete DepOrWith Transaction - Karthik
        [Authorize(Roles ="admin")]
        public IActionResult SoftDeleteDepOrWith(int id)
        {
            string sql = @"SELECT * FROM DepWithTransactions WHERE TransactionId={0}";
            string select = String.Format(sql, id);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Transaction Record does not exist";
                TempData["MsgType"] = "warning";

            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format("UPDATE DepWithTransactions SET Deleted='True',DeletedDate='{1:yyyy-MM-dd}' WHERE TransactionId={0}", id, DateTime.Now));
                if(res == 1)
                {
                    ViewData["Message"] = "Transaction successfully deleted";
                    ViewData["MsgType"] = "success";
                }
                else
                {
                    ViewData["Message"] = "Transaction could not be deleted";
                    ViewData["MsgType"] = "danger";
                }

            }
            return RedirectToAction("DepOrWithIndex");

        }
        #endregion

        #region Recover Deleted DepOrWith Transaction - Karthik
        [Authorize(Roles ="admin")]
        public IActionResult RecoverDepOrWith(int id)
        {
            string sql = @"SELECT * FROM DepWithTransactions WHERE TransactionId={0}";
            string select = String.Format(sql, id);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Transaction Record does not exist";
                TempData["MsgType"] = "warning";

            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format(@"UPDATE DepWithTransactions SET Deleted='False', DeletedDate=null WHERE TransactionId={0}", id));
                if (res == 1)
                {
                    ViewData["Message"] = "Transaction successfully recovered";
                    ViewData["MsgType"] = "success";
                    return View();
                }
                else
                {
                    ViewData["Message"] = "Transaction could not be recovered";
                    ViewData["MsgType"] = "danger";
                    return View();
                }
            }
            return RedirectToAction("DeletedDepOrWithIndex");
        }
        #endregion

        #region Permanent Delete DepOrWith Transaction - Karthik
        [Authorize(Roles ="admin")]
        public IActionResult PermanentDeleteDepOrWith(int id)
        {
            string sql = @"SELECT * FROM DepWithTransactions WHERE TransactionId={0}";
            string select = String.Format(sql, id);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Transaction Record does not exist";
                TempData["MsgType"] = "warning";

            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format(@"DELETE * FROM DepWithTransactions WHERE TransactionId={0}", id));
                if (res == 1)
                {
                    ViewData["Message"] = "Transaction successfully deleted permanently";
                    ViewData["MsgType"] = "success";
                    return View();
                }
                else
                {
                    ViewData["Message"] = "Transaction could not be deleted permanently";
                    ViewData["MsgType"] = "danger";
                    return View();
                }
            }
            return RedirectToAction("DeletedDepOrWithIndex");
        }
        #endregion
    }

}



   
