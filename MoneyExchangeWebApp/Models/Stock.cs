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
        public int AccountId { get; set; }
        [Required]
        public String ISO { get; set; }
        [Required(ErrorMessage ="You need to state an amount!")]
        public double Amount { get; set; }
    }
}
