using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyExchangeWebApp.Models
{
    public class Enquiry
    {
        [Key]
        public int EnquiryId { get; set; }
        [Required(ErrorMessage = "You need to enter your email!")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "You need to select the subject")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "You need to enter your question!")]
        public string Question { get; set; }
        public DateTime EnquiryDate { get; set; }
        public string Status { get; set; }
        [Required(ErrorMessage = "Your reply to the enquiry cannot be null")]
        public string Answer { get; set; }
        public string AnsweredBy { get; set; }
        public DateTime AnswerDate { get; set; }
    }
}
