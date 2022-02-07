using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using FYP.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace FYP.Controllers
{
   public class CandidateController : Controller
   {
      public IActionResult Index()
      {
         List<Candidate> lstCandidate =
            DBUtl.GetList<Candidate>("SELECT * FROM Candidate ORDER BY CName");
         return View(lstCandidate);
      }

      public IActionResult Display(int id)
      {
         string sql = String.Format(@"SELECT * FROM Candidate 
                                       WHERE RegNo = {0}", id);
         List<Candidate> lstCandidate = DBUtl.GetList<Candidate>(sql);
         if (lstCandidate.Count == 0)
         {
            TempData["Message"] = $"Candidate #{id} not found";
            TempData["MsgType"] = "warning";
            return RedirectToAction("Index");
         }
         else
         {
            // Get the FIRST element of the List
            Candidate cdd = lstCandidate[0];
            return View(cdd);
         }
      }

      // To Present An Emtpy Form
      [HttpGet]
      public IActionResult Create()
      {
         return View();
      }

      // To Handle Post Back Input Data 
      [HttpPost]
      public IActionResult Create(Candidate cdd, IFormFile photo)
      {
         if (!ModelState.IsValid)
         {
            ViewData["Message"] = "Invalid Input";
            ViewData["MsgType"] = "warning";
            return View(cdd);
         }
         else
         {
            cdd.PicFile = Path.GetFileName(photo.FileName);
            string fname = "candidates/" + cdd.PicFile;
            UploadFile(photo, fname);

            string sql = @"INSERT Candidate(RegNo, CName, 
                                            Gender, Height, 
                                            BirthDate, Race, 
                                            Clearance, PicFile) 
                           VALUES({0},'{1}','{2}',{3},
                          '{4:yyyy-MM-dd}','{5}','{6}','{7}')";

            string insert = 
               String.Format(sql, cdd.RegNo, cdd.CName, cdd.Gender, 
                                  cdd.Height, cdd.BirthDate, cdd.Race, 
                                  cdd.Clearance, cdd.PicFile);
            if (DBUtl.ExecSQL(insert) == 1)
            {
               TempData["Message"] = $"Candidate #{cdd.RegNo} created Successfully";
               TempData["MsgType"] = "success";
               return RedirectToAction("Index");
            }
            else
            {
               ViewData["Message"] = DBUtl.DB_Message;
               ViewData["MsgType"] = "danger";
               return View(cdd);
            }
         }
      }

      private void UploadFile(IFormFile ufile, string fname)
      {
         string fullpath = Path.Combine(_env.WebRootPath, fname);
         using (var fileStream = new FileStream(fullpath, FileMode.Create))
         {
            ufile.CopyToAsync(fileStream);
         }
      }

      private IWebHostEnvironment _env;
      public CandidateController(IWebHostEnvironment environment)
      {
         _env = environment;
      }

   }
}
