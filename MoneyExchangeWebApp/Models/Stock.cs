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
        public int Stock_id { get; set; }
        [Required]
        public string  Stock_name { get; set; }
        [Required]
        public decimal Stock_amount { get; set; }
        [Required]
        public decimal Average_rate { get; set; }
    }
}
