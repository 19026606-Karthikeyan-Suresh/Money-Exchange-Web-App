using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FYP.Models
{
    public class Stock
    {

        public string StockID { get; set; }
        public string  CurrenctName { get; set; }
        [Required(ErrorMessage = "Currency Stock must be decimal")]
        public decimal CurrencyStock { get; set; }
        [Required( ErrorMessage = "Average Rate must be decimal")]
        public decimal AverageRate { get; set; }

    }
}
