using System.ComponentModel.DataAnnotations;

namespace MoneyExchangeWebApp.Models
{
    public class BOS
    {
        [Required]
        public string bestorworst { get; set; }
        [Required]
        public int month { get; set; }
    }
}
