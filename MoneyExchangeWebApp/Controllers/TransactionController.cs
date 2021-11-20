using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using MoneyExchangeWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Claims;

namespace MoneyExchangeWebApp.Controllers
{
    public class TransactionController : Controller
    {

        #region "Transaction View ALl - Karthik";
        public IActionResult AllTransactions()
        {
            List<Transaction> tranList = DBUtl.GetList<Transaction>("SELECT * FROM Transactions");
            return View(tranList);

        }
        #endregion

        #region
        public IActionResult AddTransaction()
        {
            return (null);
        }
        #endregion
    }
}
