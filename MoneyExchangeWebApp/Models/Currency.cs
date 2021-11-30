using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyExchangeWebApp.Models
{
    public class Currency
    {
        [Required]
        public string  Currency_name { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public decimal Average_rate { get; set; }
        [Required]
        public string Created_by { get; set; }
        [Required]
        public DateTime Created_date { get; set; }
        [Required]
        public int Deleted { get; set; }
        public string Deleted_by { get; set; }
        

    }
}
