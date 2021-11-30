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
   public class AccountController : Controller
   {
        #region "Login/Logout" - Teng Yik
        [Authorize]
        public IActionResult Logoff(string returnUrl = null)
      {
         HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
         if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
         return RedirectToAction("ExchangeRates", "Currency");
      }

      [AllowAnonymous]
      public IActionResult Login(string returnUrl = null)
      {
         TempData["ReturnUrl"] = returnUrl;
         return View();
      }

      [AllowAnonymous]
      [HttpPost]
      public IActionResult Login(UserLogin user)
      {
         if (!AuthenticateUser(user.UserID, user.Password,
                               out ClaimsPrincipal principal))
         {
            ViewData["Message"] = "Incorrect User ID or Password";
            return View();
         }
         else
         {
            HttpContext.SignInAsync(
               CookieAuthenticationDefaults.AuthenticationScheme,
               principal);

            if (TempData["returnUrl"] != null)
            {
               string returnUrl = TempData["returnUrl"].ToString();
               if (Url.IsLocalUrl(returnUrl))
                  return Redirect(returnUrl);
            }

            return RedirectToAction("ExchangeRates", "Currency");
         }
      }
        #endregion

        #region "Authenticate User" - Teng Yik
        private bool AuthenticateUser(string uid, string pw,
                                    out ClaimsPrincipal principal)
      {
         principal = null;
         string sql = @"SELECT * FROM Accounts WHERE Username = '{0}' AND Password = HASHBYTES('SHA1', '{1}') ";

         string select = String.Format(sql, uid, pw);
         DataTable ds = DBUtl.GetTable(select);
         if (ds.Rows.Count == 1)
         {
            principal =
               new ClaimsPrincipal(
                  new ClaimsIdentity(
                     new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, uid),
                        new Claim(ClaimTypes.Name, ds.Rows[0]["Name"].ToString())
                     },
                     CookieAuthenticationDefaults.AuthenticationScheme));
                return true;
         }
         return false;
      }
        #endregion

        #region "Display User Accounts" - Teng Yik
        [Authorize]
        public IActionResult AccountIndex()
        {
            List<Account> accountList = DBUtl.GetList<Account>("SELECT * FROM Accounts WHERE Deleted='false'");
            return View(accountList);
        }

        #endregion

        #region "Display Deleted Accounts" - Teng Yik
        [Authorize]
        public IActionResult DeletedAccounts()
        {
            List<Account> accountList = DBUtl.GetList<Account>("SELECT * FROM Accounts WHERE deleted='true'");
            return View(accountList);
        }
        #endregion

        #region "Add User Accounts" - Teng Yik
        [Authorize]
        public IActionResult AddUsers()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUsers(Account AC)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("AddUsers");

            } else
            {
                string sql =
              @"INSERT INTO Accounts(Username, Password, Name, Role, Date_created, Deleted, Deleted_by)
              VALUES('{0}',HASHBYTES('SHA1','{1}'), '{2}', '{3}', '{4:yyyy-MM-dd}', {5}, '{6}')";

                string insert = String.Format(sql, AC.Username.EscQuote(), AC.Password.EscQuote(), AC.Name.EscQuote(), AC.Role.EscQuote(), 
                    AC.Date_created, 0, null);

                int count = DBUtl.ExecSQL(insert);
                if (count == 1)
                {
                    TempData["Message"] = "User Successfully Added.";
                    TempData["MsgType"] = "success";
                    return RedirectToAction("AccountIndex");
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                    return View("AddUsers");
                }
            }
        }

        #endregion

        #region "Edit User Accounts" - Teng Yik
        [Authorize]
        public IActionResult EditUsers(int id)
        {
            string sql = @"SELECT * FROM Accounts WHERE Account_id={0}";

            string select = String.Format(sql, id);
            List<Account> Alist = DBUtl.GetList<Account>(select);
            if (Alist.Count == 1)
            {
                Account A = Alist[0];
                return View(A);
            }
            else
            {
                TempData["Message"] = "Account does not exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("AccountIndex");
            }
        }

        [Authorize]
        [HttpPost]
        public IActionResult EditUsers(Account A)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "danger";
                return View("EditUsers", A);
            }
            else
            {
                string sql = @"UPDATE Accounts  
                              SET Password= HASHBYTES('SHA1','{1}'), Name='{2}', Role='{3}', Date_created='{4:yyyy-MM-dd}' 
                            WHERE Account_id={0}";
                string update = String.Format(sql, A.Account_id, A.Password.EscQuote(), A.Name.EscQuote(), A.Role.EscQuote(), A.Date_created);

                if (DBUtl.ExecSQL(update) == 1)
                {
                    TempData["Message"] = "Accounts Updated";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("AccountIndex");
            }
        }
        #endregion

        #region "Delete user Accounts" - Teng Yik
        [Authorize]
        public IActionResult Delete(int id)
        {
            string sql = @"SELECT * FROM Accounts WHERE Account_id={0}";
            string select = String.Format(sql, id);
            DataTable dt = DBUtl.GetTable(select);
            if (dt.Rows.Count != 1)
            {
                ViewData["Message"] = "The user account you are trying to delete, does not exist";
                ViewData["MsgType"] = "warning";
            }
            else
            {
                string sql1 = @"UPDATE Accounts  
                              SET Deleted='True', Deleted_by='{1}' WHERE Account_id={0}";

                string userid = User.Identity.Name;

                int res = DBUtl.ExecSQL(String.Format(sql1, id, userid));
                if (res == 1)
                {
                    TempData["Message"] = "Account has been deleted";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("AccountIndex");
        }

        #endregion

        #region "Recover Deleted Account" - Teng Yik
        [Authorize]
        public IActionResult RecoverAccount(int id)
        {
            string sql = @"SELECT * FROM Accounts 
                         WHERE Account_id={0}";

            string select = String.Format(sql, id);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Account does not exist";
                TempData["MsgType"] = "warning";
            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format("UPDATE Accounts SET Deleted='False', Deleted_by=null WHERE Account_id={0}", id));
                if (res == 1)
                {
                    TempData["Message"] = "Account Recovered";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("DeletedAccounts");
        }
        #endregion

    }
}