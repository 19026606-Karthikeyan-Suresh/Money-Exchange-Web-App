using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyExchangeWebApp.Models
{
    public class Stock
    {
        [Required]
        public int StockId { get; set; }
        [Required]
        public string  AccountId { get; set; }
        [Required]
        public decimal ISO { get; set; }
        [Required]
        public decimal Amount { get; set; }
    }
}
