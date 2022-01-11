namespace MoneyExchangeWebApp.Models
{
    public class ExchangeRates
    {
       public string Source_currency { get; set; }
        public string Target_currency { get; set;}
        public double Exchange_rate { get; set; }  
    }
}
