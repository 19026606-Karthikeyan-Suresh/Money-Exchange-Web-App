using System.ComponentModel.DataAnnotations;

namespace MoneyExchangeWebApp.Models
{
    public class Top5Currency
    {
        [Required(ErrorMessage = "Please choose Best or Worst")]
        public string BestOrWorst { get; set; }

        [Required(ErrorMessage = "Please choose a month")]
        public int Month { get; set; }
    }
}
