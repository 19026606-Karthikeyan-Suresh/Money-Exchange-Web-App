namespace MoneyExchangeWebApp.Models
{
    public class FAQ
    {
        public int FAQ_ID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string created_by { get; set; }
        public bool deleted { get; set; }
        public string deleted_by { get; set; }
    }
}
