using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyExchangeWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace MoneyExchangeWebApp.Controllers
{
    public class EnquiryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult FAQ()
        {
            List<FAQ> faqList = DBUtl.GetList<FAQ>("SELECT * FROM FAQ");
            return View(faqList);
        }

        public IActionResult Enquire()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Enquire(Enquiry newEnquiry)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Please fill up all the required information.";
                ViewData["MsgType"] = "warning";
            }
            else
            {
                
                string insert =
                   @"INSERT INTO Enquiries(EmailAddress, Subject, Question, EnquiryDate, Status, Answer, AnsweredBy, AnswerDate) 
                 VALUES('{0}', '{1}', '{2}','{3:yyyy-MM-dd}', '{4}', '{5}', '{6}', '{7:yyyy-MM-dd}')";
                string final = String.Format(insert, newEnquiry.EmailAddress.EscQuote(), newEnquiry.Subject.EscQuote(), newEnquiry.Question.EscQuote()
                    , DateTime.Now, "pending".EscQuote(), null, null, null);

                int result = DBUtl.ExecSQL(final);

                if (result == 1)
                {
                    ViewData["Message"] = "Enquiry Submitted! We will reply to you soonest as possible! Thank You!";
                    ViewData["MsgType"] = "success";
                }
                else
                {
                    ViewData["Message"] = DBUtl.DB_Message;
                    ViewData["MsgType"] = "danger";
                    ViewData["sql"] = final;
                }
            }
            return View();
        }

        public IActionResult AllFaqs()
        {
            List<FAQ> faqList = DBUtl.GetList<FAQ>("SELECT * FROM FAQ");
            return View(faqList);
        }

        public IActionResult GetAllEnquiries()
        {
            var enquiryList = DBUtl.GetList<Enquiry>("SELECT * FROM Enquiries");
            return Json(new { data = enquiryList });
        }

        public IActionResult EnquiryIndex()
        {
            return View();
        }

        public IActionResult EnquiryReply(int id)
        {
            string sql = @"SELECT * FROM Enquiries WHERE EnquiryId={0}";

            string select = String.Format(sql, id);
            List<Enquiry> ERlist = DBUtl.GetList<Enquiry>(select);
            if (ERlist.Count == 1)
            {
                Enquiry ER = ERlist[0];
                return View(ER);
            }
            else
            {
                TempData["Message"] = "Enquiry Record does not exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("EnquiryIndex");
            }
        }
        [HttpPost]
        public IActionResult EnquiryReply(Enquiry ER)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Input";
                ViewData["MsgType"] = "danger";
                return View("EnquiryReply", ER);
            }
            else
            {
                string sql = @"UPDATE Enquiries
                              SET Status='replied', Answer='{1}' ,AnsweredBy='{2}' WHERE EnquiryId={0} ";
                string update = String.Format(sql, ER.EnquiryId, ER.Answer, User.Identity.Name);

                if (DBUtl.ExecSQL(update) == 1)
                {
                    TempData["Message"] = "Enquiry Updated";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
                return RedirectToAction("EnquiryIndex");
            }
        }
        public IActionResult EnquireReply()
        {
            return View();
        }
        [HttpPost]
        public IActionResult EnquiryReply(Enquiry e)
        {

          
            
            string result;
            
            if (EmailUtl.SendEmail(e.EmailAddress, e.Subject, e.Answer, out result))
            {
                ViewData["Message"] = "Email Successfully Sent";
                ViewData["MsgType"] = "success";
                return RedirectToAction("EnquiryIndex");
            }
            else
            {
                ViewData["Message"] = result;
                ViewData["MsgType"] = "warning";
                return View();

            }

        }

    }
}
