using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyExchangeWebApp.Models;
using System.Collections.Generic;

namespace MoneyExchangeWebApp.Controllers
{
    public class WalletController : Controller
    {
        // GET: Admin Wallet
        public ActionResult GetAdminWallet()
        {
            if (User.IsInRole("admin"))
            {
                var CurrenciesOwned = DBUtl.GetList<Stock>(string.Format(@"SELECT * FROM Stock WHERE AccountId=1"));
                return Json(new { data = CurrenciesOwned });
            }
            else
            {
                return NotFound();
            }
        }

        public IActionResult ShowAdminWallet()
        {
            return View();
        }


        #region For User
        // Display User Waller
        public ActionResult UserWalletIndex()
        {
            List<Account> Accl = DBUtl.GetList<Account>(string.Format("SELECT * FROM Account WHERE EmailAddress='{0}'", User.Identity.Name.EscQuote()));
            string sql = @"SELECT * FROM Stock ";
            return View();
        }

        public IActionResult CheckIfUserHasWallet()
        {
            return View();
        }
        #endregion


        /*#region Unncessary Features DO NOT DELETE - Karthik
        // GET: WalletController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: WalletController/Create
        public ActionResult Create()
        {
            return View();
        }
        // POST: WalletController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(WalletIndex));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalletController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: WalletController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(WalletIndex));
            }
            catch
            {
                return View();
            }
        }

        // GET: WalletController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: WalletController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(WalletIndex));
            }
            catch
            {
                return View();
            }
        }
        #endregion*/
    }
}
