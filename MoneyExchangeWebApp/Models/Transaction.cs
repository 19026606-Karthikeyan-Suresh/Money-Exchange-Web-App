using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyExchangeWebApp.Models
{
    public class Transaction
    {
        public int Transaction_id { get; set; }
        public string Source_currency { get; set; }
        public decimal Source_amount { get; set; }
        public string Converted_currency { get; set; }
        public decimal Converted_amount { get; set; }
        public decimal Exchange_rate { get; set; }
        public DateTime Transaction_date { get; set; }

    }
}
