namespace MoneyExchangeWebApp.Models
{
    public class ExchangeRates
    {
       public string BaseCurrency { get; set; }
        public string QuoteCurrency { get; set;}
        public double ExchangeRate { get; set; }  
    }
}
