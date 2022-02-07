using System.ComponentModel.DataAnnotations;

namespace KachingExpress.Models
{
    public class ConvTransaction
    {
        public int TransactionId { get; set; }
        [Required]
        public string BaseCurrency { get; set; }
        [Required]
        public decimal BaseAmount { get; set; }
        [Required]
        public string QuoteCurrency { get; set; }
        [Required]
        public decimal QuoteAmount { get; set; }
        [Required]
        public decimal ExchangeRate { get; set; }
        [Required]
        public DateTime TransactionDate { get; set; }
        [Required]
        public string DoneBy { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public int Deleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}
