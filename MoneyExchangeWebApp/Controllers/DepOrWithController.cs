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
        public IActionResult DOWTransactions(int id)
        {
            string sql = @"SELECT * FROM DepWithTransactions WHERE StockId={0}";
            List<DepWithTransactions> DWTlist = DBUtl.GetList<DepWithTransactions>(sql, id);
            return View(DWTlist);
        }
        #endregion
    }

}



   
