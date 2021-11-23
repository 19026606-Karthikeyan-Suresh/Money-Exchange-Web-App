using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FYP.Models;
using System;
using System.Data;
using System.Security.Claims;

namespace FYP.Controllers
{
   public class AccountController : Controller
   {
      [Authorize]
      public IActionResult Logoff(string returnUrl = null)
      {
         HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
         if (Url.IsLocalUrl(returnUrl))
            return Redirect(returnUrl);
         return RedirectToAction("Index", "Travel");
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

            return RedirectToAction("Index", "Travel");
         }
      }

      private bool AuthenticateUser(string uid, string pw,
                                    out ClaimsPrincipal principal)
      {
         principal = null;

         // TODO L08 Task 1 - Provide Login SELECT Statement
         string sql = @"SELECT * FROM TravelUser WHERE UserId = '{0}' AND UserPw = HASHBYTES('SHA1', '{1}') ";

         string select = String.Format(sql, uid, pw);
         DataTable ds = DBUtl.GetTable(select);
         if (ds.Rows.Count == 1)
         {
            principal =
               new ClaimsPrincipal(
                  new ClaimsIdentity(
                     new Claim[] {
                        new Claim(ClaimTypes.NameIdentifier, uid),
                        new Claim(ClaimTypes.Name, ds.Rows[0]["FullName"].ToString())
                     },
                     CookieAuthenticationDefaults.AuthenticationScheme));
            return true;
         }
         return false;
      }

   }
}