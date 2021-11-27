using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace FYP.Models
{
   public class Account
   {
        public string username { get; set; }
        public string password { get; set; }
        public string name { get; set; }
        public string role { get; set; }
        public DateTime dob { get; set; }
        public int deleted { get; set; }
        public string deleted_by { get; set; }
    }
}


