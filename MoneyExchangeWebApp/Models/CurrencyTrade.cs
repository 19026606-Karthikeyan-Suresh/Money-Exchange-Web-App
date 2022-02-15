using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MoneyExchangeWebApp.Models
{
    public class CurrencyTrade
    {
        [Key]
        public int TransactionId { get; set; }
        [Required]
        public string BaseCurrency { get; set; }
        [Required]
        public double BaseAmount { get; set; }
        [Required]
        public string QuoteCurrency { get; set; }
        [Required]
        public double QuoteAmount { get; set; }
        [Required]
        public double ExchangeRate { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        public string DoneBy { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public int Deleted { get; set; }
        public string DeletedBy { get; set;}
        public DateTime DeletedDate { get; set; }

    }
}
