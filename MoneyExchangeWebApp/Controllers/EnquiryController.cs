using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyExchangeWebApp.Models;
using Microsoft.AspNetCore.Authorization;
using System.Net.Mail;
using System.Security.Claims;

namespace MoneyExchangeWebApp.Controllers
{
    public class EnquiryController : Controller
    {
        #region View All Enquiries - Jasper
        [Authorize(Roles = "staff,admin")]
        public IActionResult EnquiryIndex()
        {
            List<Enquiry> enquiryList = DBUtl.GetList<Enquiry>("SELECT * FROM Enquiries");
            return View(enquiryList);
        }
        #endregion

        #region View all FAQs (Not operational) - Karthik
        [AllowAnonymous]
        public IActionResult FAQ()
        {
            List<FAQ> faqList = DBUtl.GetList<FAQ>("SELECT * FROM FAQ");
            return View(faqList);
        }
        #endregion

        #region Send an enquiry - Jasper
        [AllowAnonymous]
        public IActionResult Enquire()
        {
            return View();
        }


        [AllowAnonymous]
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
                 VALUES('{0}', '{1}', '{2}','{3:yyyy-MM-dd HH:mm:ss}', '{4}', '{5}', '{6}', '{7}')";
                string final = String.Format(insert, newEnquiry.EmailAddress.EscQuote(), newEnquiry.Subject.EscQuote(), newEnquiry.Question.EscQuote()
                    , DateTime.Now, "Pending".EscQuote(), null, null, null);

                int result = DBUtl.ExecSQL(final);

                if (result == 1)
                {
                    TempData["Message"] = "Enquiry Submitted! We will reply to you soonest as possible! Thank You!";
                    TempData["MsgType"] = "success";
                }
                else
                {
                    TempData["Message"] = DBUtl.DB_Message;
                    TempData["MsgType"] = "danger";
                }
            }
            return RedirectToAction("Index", "Home");
        }
        #endregion

        #region Reply to an enquiry - Jasper
        [Authorize(Roles = "staff,admin")]
        public IActionResult EnquiryReply(int id)
        {
            string sql = @"SELECT * FROM Enquiries WHERE EnquiryId={0}";

            string select = String.Format(sql, id);
            List<Enquiry> ERlist = DBUtl.GetList<Enquiry>(select);
            if (ERlist.Count == 1)
            {
                Enquiry ER = ERlist[0];
                if (ER.Status.Equals("Replied"))
                {
                    ViewData["Status"] = "Replied";
                }
                else
                {
                    ViewData["Status"] = "Pending";
                }
                return View(ER);
            }
            else
            {
                TempData["Message"] = "Enquiry Record does not exist";
                TempData["MsgType"] = "warning";
                return RedirectToAction("EnquiryIndex");
            }
        }

        [Authorize(Roles = "staff,admin")]
        [HttpPost]
        public IActionResult EnquiryReply(Enquiry ER)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Message"] = "Invalid Model";
                ViewData["MsgType"] = "danger";
                return View();
            }
            else
            {
                string template = @"To User with the Email {0},
                               
                 Welcome to KachingXpress!

                 We have received your enquiry of: {1}
                 Our Team Member has attended to your enquiry and has given a reply of: {2}
                 *If you have anymore enquiries, please do submit another enquiry on our platform. Thank you for using KachingXpress
                               
                 Replying Member: {3}
                 KachingXpress";

                string message = String.Format(template, ER.EmailAddress, ER.Question, ER.Answer, User.FindFirstValue(ClaimTypes.NameIdentifier));
                Boolean result = SendMail(ER.EmailAddress, ER.Subject, message);

                if (result == true)
                {
                    TempData["EmailSent"] = "Email Successfully Sent";
                    string sql = @"UPDATE Enquiries
                                  SET Status='Replied', Answer='{1}' ,AnsweredBy='{2}', AnswerDate='{3:yyyy-MM-dd HH:mm:ss}' WHERE EnquiryId={0} ";
                    string update = String.Format(sql, ER.EnquiryId, ER.Answer.EscQuote(), User.FindFirstValue(ClaimTypes.NameIdentifier), DateTime.Now);

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
                }

                else
                {
                    TempData["EmailSent"] = "Error Sending Email";
                }
                return RedirectToAction("EnquiryIndex");
            }
        }
        #endregion

        #region Send Email Method used to reply to enquiries - Jasper
        private Boolean SendMail(string to, string subject, string msg)
        {
            string error = "";
            try
            {
                MailMessage mail = new MailMessage();
                SmtpClient smtp = new SmtpClient("smtp.outlook.com");

                mail.From = new MailAddress("kachingexchange@outlook.com");
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = msg;

                smtp.Port = 587;
                smtp.Credentials = new System.Net.NetworkCredential("kachingexchange@outlook.com", "m0n3y3x!");
                smtp.EnableSsl = true;

                smtp.Send(mail);
            }
            catch (Exception e)
            {
                error = e.Message;

            }
            if (error == "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
