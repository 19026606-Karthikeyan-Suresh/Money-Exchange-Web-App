using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyExchangeWebApp.Models;
using Microsoft.AspNetCore.Authorization;

namespace MoneyExchangeWebApp.Controllers
{
    public class EnquiriesController : Controller
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
                   @"INSERT INTO Enquiries(visitor_email_address, description, enquiry_date, status, answered_by, deleted, deleted_by) 
                 VALUES('{0}', '{1}', '{2:dd/MM/yyyy}', {3}, '{4}', {5}, '{6}')";
                string final = String.Format(@"INSERT INTO Enquiries(visitor_email_address, description, enquiry_date, status, answered_by, deleted, deleted_by) 
                 VALUES('{0}', '{1}', '{2:dd/MM/yyyy}', {3}, '{4}', {5}, '{6}')", newEnquiry.Visitor_email_address, newEnquiry.Description, newEnquiry.Enquiry_date
                    , newEnquiry.Status, newEnquiry.Answered_by, newEnquiry.Deleted, newEnquiry.Deleted_by);
                int result = DBUtl.ExecSQL(insert, newEnquiry.Visitor_email_address, newEnquiry.Description, newEnquiry.Enquiry_date
                    , newEnquiry.Status, newEnquiry.Answered_by, newEnquiry.Deleted, newEnquiry.Deleted_by);

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

        public IActionResult AllEnquiries()
        {
            List<Enquiry> enquiryList = DBUtl.GetList<Enquiry>("SELECT * FROM Enquiries WHERE Deleted ='False'");
            return View(enquiryList);
        }

        [Authorize]
        public IActionResult ReplyToEnquiry(int Enquiry_id)
        {
            string sql = @"SELECT * FROM Enquiries WHERE Enquiry_id = {0}";
            string select = String.Format(sql, Enquiry_id);
            List<Enquiry> EnquiryList = DBUtl.GetList<Enquiry>(select);
            if (EnquiryList.Count == 1)
            {
                Enquiry EL = EnquiryList[0];
                return View(EL);
            }
            else
            {
                TempData["Message"] = "Enquiry could not be found";
                TempData["MsgType"] = "warning";
            }
            return View();
        }
    }
}
