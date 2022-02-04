using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace KachingExpress.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        [Required(ErrorMessage = "Email Address field cannot be empty!")]
        [Remote(action: "VerifyEmail", controller: "Account")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "You need to set a password to log in!")]
        public string Password { get; set; }
        [Required(ErrorMessage = "First Name field cannot be empty!")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Last Name field cannot be empty!")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Address field cannot be empty!")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Phone number field cannot be empty!")]
        public int PhoneNumber { get; set; }
        [Required(ErrorMessage = "You need to select a gender!")]
        public string Gender { get; set; }
        [Required(ErrorMessage = "Date of Birth field cannot be empty!")]
        public DateTime DOB { get; set; }
        public string Role { get; set; }
        public DateTime DateCreated { get; set; }
        public string EditedBy { get; set; }
        public DateTime DateEdited { get; set; }
        public int Deleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DateDeleted { get; set; }
        

    }
}


