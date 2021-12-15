using System;
using System.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoneyExchangeWebApp.Models;

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
                 VALUES('{0}', '{1}', '{2:dd/MM/yyyy}', {3}, '{4}', {5}, '{6}')", newEnquiry.visitor_email_address, newEnquiry.description, newEnquiry.enquiry_date
                    , newEnquiry.status, newEnquiry.answered_by, newEnquiry.deleted, newEnquiry.deleted_by);
                int result = DBUtl.ExecSQL(insert, newEnquiry.visitor_email_address, newEnquiry.description, newEnquiry.enquiry_date
                    , newEnquiry.status, newEnquiry.answered_by, newEnquiry.deleted, newEnquiry.deleted_by);

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
        public IActionResult AllEnquiries()
        {
            List<Enquiry> faqList = DBUtl.GetList<Enquiry>("SELECT * FROM Enquiries");
            return View(faqList);
        }

        public IActionResult AllEnquiry()
        {
            List<Enquiry> enquiryList = DBUtl.GetList<Enquiry>("SELECT * FROM Enquiries");
            return View(enquiryList);
        }
    }
}
