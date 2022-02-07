namespace KachingExpress.Models
{
    public class DepWithTransactions
    {
        public int TransactionId { get; set; }
        public int StockId { get; set; }
        public string ISO { get; set; }
        public string DepOrWith { get; set; }
        public double Amount { get; set; }
        public string DoneBy { get; set; }
        public DateTime TransactionDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public int Deleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }
    }
}
