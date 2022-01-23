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
                 VALUES('{0}', '{1}', '{2}','{3:yyyy-MM-dd}', '{3}', '{4}', '{5}', '{6:yyyy-MM-dd}')";
                string final = String.Format(insert, newEnquiry.EmailAddress.EscQuote(), newEnquiry.Subject.EscQuote(), newEnquiry.Question.EscQuote()
                    , newEnquiry.EnquiryDate, newEnquiry.Status.EscQuote(), newEnquiry.Answer.EscQuote(),newEnquiry.AnsweredBy.EscQuote(), newEnquiry.AnswerDate);

                int result = DBUtl.ExecSQL(insert);

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

        public IActionResult EnquiryIndex()
        {
            List<Enquiry> enquiryList = DBUtl.GetList<Enquiry>("SELECT * FROM Enquiries WHERE Deleted ='False'");
            return View(enquiryList);
        }

        public IActionResult EnquiryReply(int id)
        {
            string sql = @"SELECT * FROM Enquiries WHERE Enquiry_id={0}";

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
        public IActionResult EnquireReply(Enquiry ER)
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
                              SET Status='replied', Answer='{1}' ,Answered_by='{2}' WHERE Enquiry_id={0} ";
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
                return RedirectToAction("EnquiryList");
            }
        }
    }
}
