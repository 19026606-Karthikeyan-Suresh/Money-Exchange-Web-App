using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class Transaction
    {
        public int transaction_id { get; set; }
        public char Source_currency { get; set; }
        public decimal Source_amount { get; set; }
        public char Converted_currency { get; set; }
        public decimal Converted_amount { get; set; }
        public DateTime Transaction_date { get; set; }
        public string Comments { get; set; }

    }
}
