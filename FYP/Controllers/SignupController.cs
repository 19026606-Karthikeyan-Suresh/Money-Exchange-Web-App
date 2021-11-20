using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;

namespace FYP.Controllers
{
    public class SignupController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Thanks()
        {
            IFormCollection form = HttpContext.Request.Form;
            string username = form["Username"].ToString().Trim();
            string password = form["Password"].ToString().Trim();

            ViewData["Username"] = username;
            ViewData["Password"] = password;

            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }

        public IActionResult GetUserAccounts()
        {
            ViewData["dbtable"] = DBUtl.GetTable("SELECT * FROM Accounts");
            return View();
        }

        public IActionResult AddUserAccounts()
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

            string insert = String.Format(@"INSERT INTO Accounts(account_id, username, password, name, role, dob)
              VALUES('{0}','{1}', '{2}', '{3}','{4}', '{5}')", accountID, userName, passWord, namE, rolE, doB);

            int count = DBUtl.ExecSQL(insert);
            if (count == 1)
            {
                TempData["Message"] = "User Successfully Added.";
                TempData["MsgType"] = "success";
                return RedirectToAction("");
            }
            else
            {
                ViewData["Message"] = DBUtl.DB_Message;
                ViewData["MsgType"] = "danger";
                return View("AddUserAccounts");
            }

        }

        public IActionResult EditUserAccounts()
        {

            return View();
        }

        public IActionResult DeleteUserAccounts()
        {

            return View();
        }

        public IActionResult SuccessAddition()
        {
            return View();
        }

        public IActionResult SuccessDelete()
        {
            return View();
        }

        public IActionResult SuccessUpdate()
        {
            return View();
        }

    }
}
