using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyExchangeWebApp.Models
{
    public class Enquiry
    {
        public int EnquiryId { get; set; }
        [Required(ErrorMessage = "You need to enter your email!")]
        public string EmailAddress { get; set; }
        [Required(ErrorMessage = "You need to select the subject")]
        public string Subject { get; set; }
        [Required(ErrorMessage = "You need to enter your question!")]
        public string Question { get; set; }
        public DateTime EnquiryDate { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public string Answer { get; set; }
        public string AnsweredBy { get; set; }
        public DateTime AnswerDate { get; set; }
     
    }
}
