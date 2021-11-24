using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FYP.Models;
using System;
using System.Data;
using System.Security.Claims;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace FYP.Controllers
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
         // TODO L08 Task 1 - Provide Login SELECT Statement
         string sql = @"SELECT * FROM Accounts WHERE username = '{0}' AND password = HASHBYTES('SHA1', '{1}') ";

         string select = String.Format(sql, uid, pw);
         DataTable ds = DBUtl.GetTable(select);
         if (ds.Rows.Count == 1)
         {
            principal =
               new ClaimsPrincipal(
                  new ClaimsIdentity(
                     new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, uid),
                        new Claim(ClaimTypes.Name, ds.Rows[0]["name"].ToString())
                     },
                     CookieAuthenticationDefaults.AuthenticationScheme));
            return true;
         }
         return false;
      }
        #endregion

        #region "Display User Accounts" - Teng Yik
        [Authorize]
        public IActionResult GetUserAccounts()
        {
            ViewData["dbtable"] = DBUtl.GetTable("SELECT * FROM Accounts");
            return View();
        }
        #endregion

        #region "Add User Accounts" - Teng Yik
        [Authorize]
        public IActionResult AddUserAccounts()
        {
            return View();
        }

        public IActionResult AddUserAccountsPost()
        {
            IFormCollection form = HttpContext.Request.Form;
            string accountID = form["account_id"].ToString().Trim();
            string userName = form["username"].ToString().Trim();
            string passWord = form["password"].ToString().Trim();
            string namE = form["name"].ToString().Trim();
            string rolE = form["role"].ToString().Trim();
            string doB = form["dob"].ToString().Trim();

            if (ValidUtl.CheckIfEmpty(accountID, userName, passWord, namE, rolE, doB))
            {
                ViewData["Message"] = "Please enter all fields";
                ViewData["MsgType"] = "warning";
                return View("AddUserAccounts");
            }

            if (!accountID.IsInteger())
            {
                ViewData["Message"] = "Account ID must be an integer";
                ViewData["MsgType"] = "warning";
            }

            string sql = @"INSERT INTO Accounts(account_id, username, password, name, role, dob)
              VALUES({0},'{1}', HASHBYTES('SHA1', '{2}'), '{3}','{4}', '{5}')";

            string insert = String.Format(sql, accountID, userName, passWord, namE, rolE, doB);

            int count = DBUtl.ExecSQL(insert);
            if (count == 1)
            {
                TempData["Message"] = "User Successfully Added.";
                TempData["MsgType"] = "success";
                return RedirectToAction("GetUserAccounts");
            }
            else
            {
                ViewData["Message"] = DBUtl.DB_Message;
                ViewData["MsgType"] = "danger";
                return View("AddUserAccounts");
            }

        }
        #endregion

        #region "Edit User Accounts" - Teng Yik
        [Authorize]
        public IActionResult EditUserAccounts()
        {
            return View("EditUserAccounts");
        }

        public IActionResult NewForm()
        {
            return View();
        }
        public IActionResult EditUserAccountsPost()
        {
            IFormCollection form = HttpContext.Request.Form;

            string userName = form["username"].ToString().Trim();
            string passWord = form["password"].ToString().Trim();
            string namE = form["name"].ToString().Trim();
            string rolE = form["role"].ToString().Trim();
            string doB = form["dob"].ToString().Trim();

            if (ValidUtl.CheckIfEmpty(userName, passWord, namE, rolE, doB))
            {
                ViewData["Message"] = "Please enter all fields";
                ViewData["MsgType"] = "warning";
                return View("NewForm");
            }

            string sql = @"UPDATE Accounts SET (username = '{0}', password = '{1}', name = '{2}', role = '{3}', dob = '{4}')";
            string update = String.Format(sql, userName, passWord, namE, rolE, doB);

            int count = DBUtl.ExecSQL(update);
            if (count == 1)
            {
                TempData["Message"] = "User has been successfully updated.";
                TempData["MsgType"] = "success";
                return RedirectToAction("GetUserAccounts");
            }
            else
            {
                ViewData["Message"] = DBUtl.DB_Message;
                ViewData["MsgType"] = "danger";
                return View("NewForm");
            }
        }
        #endregion

        #region "Delete user Accounts" - Teng Yik
        [Authorize]
        public IActionResult DeleteUserAccounts()
        {
            return View("DeleteUserAccounts");
        }

        public IActionResult DeleteUserAccountsPost()
        {
            IFormCollection form = HttpContext.Request.Form;
            string accountID = form["account_id"].ToString().Trim();

            if (!accountID.IsInteger())
            {
                ViewData["Message"] = "Account ID must be an integer";
                ViewData["MsgType"] = "warning";
                return View("DeleteUserAccounts");
            }

            string sql = @"SELECT * FROM Accounts WHERE account_id={0}";
            string select = String.Format(sql, accountID);
            DataTable dt = DBUtl.GetTable(select);

            if (dt.Rows.Count == 0)
            {
                ViewData["Message"] = "Account ID not found";
                ViewData["MsgType"] = "warning";
                return View("DeleteUserAccouns");
            }

            sql = @"DELETE Accounts WHERE account_id={0}";
            string delete = String.Format(sql, accountID);

            int count = DBUtl.ExecSQL(delete);
            if (count == 1)
            {
                ViewData["Message"] = "User deleted";
                ViewData["MsgType"] = "success";
            }
            else
            {
                ViewData["Message"] = DBUtl.DB_Message;
                ViewData["MsgType"] = "danger";
            }

            return View("DeleteUserAccounts");

        }
        #endregion

    }
}