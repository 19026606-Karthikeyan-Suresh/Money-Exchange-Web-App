using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoneyExchangeWebApp.Models;
using System.Collections.Generic;

namespace MoneyExchangeWebApp.Controllers
{
    public class FAQController : Controller
    {
        #region FAQ index - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult FAQIndex()
        {
            List<FAQ> Flist = DBUtl.GetList<FAQ>(@"SELECT * FROM FAQ");
            return View(Flist);
        }
        

        [Authorize(Roles = "admin")]
        public IActionResult GetAllFAQs()
        {
            var Flist = DBUtl.GetList<FAQ>(@"SELECT * FROM FAQ");
            return Json(new { data = Flist });
        }
        #endregion

        #region Create FAQ - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult CreateFAQ()
        {
            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public IActionResult CreateFAQ(FAQ f)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Model is invalid";
                ViewData["MsgType"] = "danger";
                return View();
            }
            else
            {
                string sql = @"INSERT INTO FAQ(Question, Answer) VALUES ('{0}', '{1}')";
                int res = DBUtl.ExecSQL(sql, f.Question, f.Answer);
                if (res == 1)
                {
                    ViewData["Message"] = "FAQ successfully created";
                    ViewData["MsgType"] = "success";
                    return View("FAQIndex");
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                    return View();
                }
            }
        }
        #endregion

        #region Edit FAQ - Karthik
        [Authorize(Roles = "admin")]
        public IActionResult FAQEdit(int id)
        {
            List<FAQ> Flist = DBUtl.GetList<FAQ>(@"SELECT * FROM FAQ WHERE FaqId={0}", id);
            if (Flist.Count == 1)
            {
                FAQ F1 = Flist[0];
                return View(F1);
            }
            else
            {
                ViewData["Message"] = "Faq not found";
                ViewData["MsgType"] = "warning";
                return View("FAQIndex");
            }
        }

        [Authorize(Roles = "admin")]
        public IActionResult FAQEdit(FAQ f)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "warning";
                return View("FAQEdit", f);
            }
            else
            {
                string sql = @"UPDATE FAQ SET Question='{1}', Answer='{2}' WHERE TransactionId={0}";
                string update = string.Format(sql, f.Question, f.Answer);

                if (DBUtl.ExecSQL(update) == 1)
                {
                    TempData["success"] = "FAQ record Updated";
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                }
                return RedirectToAction("FAQIndex");
            }
        }
        #endregion

        #region Delete FAQ - Karthik
        [Authorize(Roles = "admin")]
        [HttpDelete]
        public IActionResult FAQDelete(int id)
        {
            string sql = @"DELETE * FROM FAQ WHERE FaqId={0}";
            int res = DBUtl.ExecSQL(sql, id);
            if (res == 1)
            {
                TempData["Message"] = "FAQ Deleted!";
                TempData["MsgType"] = "success";
                return View();
            }
            else
            {
                TempData["Message"] = "Delete unsuccessful";
                TempData["MsgType"] = "danger";
                return View();
            }
        }
        #endregion

    }
}
