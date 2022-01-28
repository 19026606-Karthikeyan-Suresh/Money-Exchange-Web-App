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
            return View("_Login");
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(UserLogin user)
        {
            if (!AuthenticateUser(user.UserID, user.Password,
                                  out ClaimsPrincipal principal))
            {
                TempData["Message"] = "Incorrect Email Address or Password";
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
            string sql = @"SELECT * FROM Accounts WHERE EmailAddress = '{0}' AND Password = HASHBYTES('SHA1', '{1}') ";

            string select = String.Format(sql, uid, pw);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count == 1)
            {
                principal =
                   new ClaimsPrincipal(
                      new ClaimsIdentity(
                         new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, uid),
                        new Claim(ClaimTypes.Name, ds.Rows[0]["EmailAddress"].ToString()),
                        new Claim(ClaimTypes.Role, ds.Rows[0]["Role"].ToString())
                         },
                         CookieAuthenticationDefaults.AuthenticationScheme)); ;
                return true;
            }
            return false;
        }
        #endregion

        #region "Get Account List" - Karthik
        [HttpGet]
        [Authorize(Roles = "admin,staff")]
        public IActionResult GetAll()
        {
            string sql;
            if (User.IsInRole("staff"))
            {
                sql = @"SELECT * FROM Accounts WHERE Deleted='False' AND role='user'";

            }
            else
            {
                sql = @"SELECT * FROM Accounts WHERE Deleted='False' AND role!='admin'";
            }
            var AccountList = DBUtl.GetList<Account>(sql);
            return Json(new { data = AccountList });
        }

        [Authorize(Roles = "admin,staff")]
        public IActionResult AccountIndex()
        {
            return View();
        }

        #endregion

        #region "Display Deleted Accounts" - Teng Yik
        [Authorize(Roles = "Admin")]
        public IActionResult DeletedAccounts()
        {
            List<Account> accountList = DBUtl.GetList<Account>("SELECT * FROM Accounts WHERE deleted='true'");
            return View(accountList);
        }
        #endregion

        #region "Add User Accounts" - Teng Yik
        [Authorize(Roles = "staff, admin")]
        public IActionResult AddAccount()
        {
            return View();
        }

        [Authorize(Roles = "staff, admin")]
        [HttpPost]
        public IActionResult AddAccount(Account AC)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("AddAccount");

            }
            else
            {
                string sql =
              @"INSERT INTO Accounts(EmailAddress, Password, FirstName, LastName, Address, PhoneNumber, 
                Gender, DOB, Role, DateCreated, Deleted, DeletedBy, DateDeleted)
                VALUES('{0}',HASHBYTES('SHA1','{1}'), '{2}', '{3}', '{4}', {5}, '{6}', '{7:yyyy-MM-dd}', '{8}', '{9:yyyy-MM-dd}', '{10}', '{11}', '{12:yyyy-MM-dd}')";

                string insert;
                if (User.IsInRole("staff"))
                {
                    insert = String.Format(sql, AC.EmailAddress.EscQuote(), AC.Password.EscQuote(), AC.FirstName.EscQuote(),
                    AC.LastName.EscQuote(), AC.Address.EscQuote(), AC.PhoneNumber, AC.Gender.EscQuote(), AC.DOB, "user".EscQuote(),
                    DateTime.Now, 0, null, null);

                }
                else
                {
                    insert = String.Format(sql, AC.EmailAddress.EscQuote(), AC.Password.EscQuote(), AC.FirstName.EscQuote(),
                    AC.LastName.EscQuote(), AC.Address.EscQuote(), AC.PhoneNumber, AC.Gender.EscQuote(), AC.DOB, "staff".EscQuote(),
                    DateTime.Now, 0, null, null);
                }


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
                    return View("AddAccount");
                }
            }
        }

        #endregion

        #region "Edit User Accounts" - Teng Yik
        //GET
        [Authorize(Roles = "staff, admin")]
        public IActionResult EditUsers(int id)
        {
            //Filter types of Users
            string sql;
            if (User.IsInRole("staff"))
            {
                sql = @"SELECT * FROM Accounts WHERE AccountId={0} AND role='user'";
            }
            else
            {
                sql = @"SELECT * FROM Accounts WHERE AccountId={0}";
            }

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

        //POST
        [Authorize(Roles = "staff,admin")]
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
                              SET EmailAddress='{1}', FirstName='{2}', LastName='{3}', 
                              Address='{4}', PhoneNumber={5}, Gender='{6}', DOB='{7:yyyy-MM-dd}'
                            WHERE AccountId={0}";
                string update = String.Format(sql, A.AccountId, A.EmailAddress.EscQuote(), A.FirstName.EscQuote(),
                    A.LastName.EscQuote(), A.Address.EscQuote(), A.PhoneNumber, A.Gender.EscQuote(), A.DOB);

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
        [Authorize(Roles = "staff, admin")]
        public IActionResult Delete(int id)
        {
            string sql = @"SELECT * FROM Accounts WHERE AccountId={0}";
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
                              SET Deleted='True', DeletedBy='{1}', DateDeleted='{2:yyyy-MM-dd}' WHERE AccountId={0}";

                string userid = User.Identity.Name;

                int res = DBUtl.ExecSQL(String.Format(sql1, id, userid, DateTime.Now.ToShortDateString()));
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
            return RedirectToAction("AllAccounts");
        }

        #endregion

        #region "Recover Deleted Account" - Teng Yik
        [Authorize(Roles = "admin")]
        public IActionResult RecoverAccount(int id)
        {
            string sql = @"SELECT * FROM Accounts 
                         WHERE AccountId={0}";

            string select = String.Format(sql, id);
            DataTable ds = DBUtl.GetTable(select);
            if (ds.Rows.Count != 1)
            {
                TempData["Message"] = "Account does not exist";
                TempData["MsgType"] = "warning";
            }
            else
            {
                int res = DBUtl.ExecSQL(String.Format("UPDATE Accounts SET Deleted='False', DeletedBy=null, DateDeleted=null WHERE Account_id={0}", id));
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

        #region "Forgot Password" - Teng Yik
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ForgotPassword(IFormCollection form)
        {
            string email = form["Email"].ToString().Trim();
            string sql = @"SELECT * FROM Accounts WHERE EmailAddress = '{0}'";
            string select = String.Format(sql, email);
            List<Account> AccList = DBUtl.GetList<Account>(select);
            int id = AccList[0].AccountId;

            if (AccList.Count != 1)
            {
                ViewData["Message"] = "Email does not exist in the database.";
                ViewData["MsgType"] = "danger";
                return View();
            }
            else
            {
                ViewData["Message"] = "An email has been sent to you to reset your " +
                                      "password";
                ViewData["MsgType"] = "success";

                string title = "reset password";
                string template = @"Hi, '{0}', here's the link to reset your password.
                                 <a href='http://localhost:5165/Account/ResetPassword'> 
                                 Reset password </a>";

                string body = String.Format(template, AccList[0].FirstName.EscQuote());
                string result;
                if (EmailUtl.SendEmail("tengyik1763@gmail.com", title, body, out result))
                {
                    ViewData["Message"] = "Email Successfully Sent";
                    ViewData["MsgType"] = "success";
                }
                else
                {
                    ViewData["Message"] = result;
                    ViewData["MsgType"] = "warning";
                }

                return View();
            }

        }
        #endregion

        #region "Reset Password" - Teng Yik
        public IActionResult ResetPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(ResetPassword rs)
        {
            if (!ModelState.IsValid)
            {
                TempData["Message"] = "Password not changed";
                TempData["MsgType"] = "danger";
            }
            else
            {
                string sql1 = @"SELECT * FROM Accounts WHERE EmailAddress = '{0}'";
                string select = String.Format(sql1, rs.Email.EscQuote());
                List<Account> AccList = DBUtl.GetList<Account>(select);

                int userid = AccList[0].AccountId;



                if (AccList.Count == 0)
                {
                    TempData["Message"] = "Account cannot be found";
                    TempData["MsgType"] = "danger";
                }
                else
                {
                    string sql = @"UPDATE Accounts SET Password = HASHBYTES('SHA1', '{0}') WHERE AccountId = {1}";
                    string update = String.Format(sql, rs.Password.EscQuote(), userid);

                    if (DBUtl.ExecSQL(update) == 1)
                    {
                        TempData["Message"] = "Password successfully changed.";
                        TempData["MsgType"] = "success";
                    }
                    else
                    {
                        TempData["Message"] = "Password is incorrect";
                        TempData["MsgType"] = "warning";
                    }
                }
            }
            return View();
        }
        #endregion

        #region Register - Teng Yik
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult RegisterPost(Account A)
        {

            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "danger";
                return View("Register", A);
            }
            else
            {
                string sql = @"INSERT INTO Accounts (EmailAddress, Password, FirstName, LastName, Address, PhoneNumber,
                               Gender, DOB, Role, DateCreated, EditedBy, EditedDate, Deleted, DeletedBy, DateDeleted) 
                               VALUES('{0}', HASHBYTES('SHA1','{1}'), '{2}', '{3}', '{4}', {5}, '{6}', '{7:yyyy-MM-dd}', '{8}', 
                               '{9:yyyy-MM-dd}', '{10}', '{11:yyyy-MM-dd}', {12}, '{13}', '{14:yyyy-MM-dd}'";



                string insert = String.Format(sql, A.EmailAddress.EscQuote(), A.Password.EscQuote(), A.FirstName.EscQuote(),
                    A.LastName.EscQuote(), A.Address.EscQuote(), A.PhoneNumber, A.Gender.EscQuote(), A.DOB, "user", 
                      A.DateCreated, null, null, 0, null, null);


                if (DBUtl.ExecSQL(insert) == 1)
                {
                    TempData["Message"] = "Accounts Updated";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("_Login");
            }
            #endregion
        }
    }
}

