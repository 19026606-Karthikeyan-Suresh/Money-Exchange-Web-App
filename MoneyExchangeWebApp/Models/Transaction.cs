using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyExchangeWebApp.Models
{
    public class Transaction
    {
        [Required]
        public int Transaction_id { get; set; }
        [Required]
        public string Source_currency { get; set; }
        [Required]
        public decimal Source_amount { get; set; }
        [Required]
        public string Converted_currency { get; set; }
        [Required]
        public decimal Exchange_rate { get; set; }
        [Required]
        public decimal Converted_amount { get; set; }
        [Required]
        public DateTime Transaction_date { get; set; }

    }
}
