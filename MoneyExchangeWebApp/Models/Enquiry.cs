using System;
using System.ComponentModel.DataAnnotations;

namespace MoneyExchangeWebApp.Models
{
    public class Enquiry
    {
        [Key]
        public int Enquiry_id { get; set; }
        [Required(ErrorMessage = "You need to enter your email!")]
        public string Visitor_email_address { get; set; }
        [Required(ErrorMessage = "You need to state what is your enquiry")]
        public string Description { get; set; }
        public DateTime Enquiry_date { get; set; } = DateTime.Now;
        public string Status { get; set; }
        public string Answered_by { get; set; }
        public int Deleted { get; set; }
        public string Deleted_by { get; set; }
    }
}
