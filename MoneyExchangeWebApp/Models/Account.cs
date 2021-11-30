using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyExchangeWebApp.Models
{
    public class Account
    {
        [Required]
        public int Account_id {get; set;}
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Role { get; set; }
        [Required]
        public DateTime Date_created { get; set; }
        [Required]
        public int Deleted { get; set; }
        public string Deleted_by { get; set; }
    }
}


