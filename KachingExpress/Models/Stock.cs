using System.ComponentModel.DataAnnotations;

namespace KachingExpress.Models
{
    public class Stock
    {
        public int StockId { get; set; }
        [Required]
        public int AccountId { get; set; }
        [Required(ErrorMessage = "You need to select a Currency!")]
        public String ISO { get; set; }
        [Required(ErrorMessage = "You need to state an amount!")]
        public double Amount { get; set; }
    }
}
