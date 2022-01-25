using System;

namespace MoneyExchangeWebApp.Models
{
    public class DepWithTransactions
    {
        public int TransactionId { get; set; }
        public string EmailAddress { get; set; }
        public int DepOrWith { get; set; }
        public string Currency { get; set; }
        public float DepositAmt { get; set; }
        public float WithdrawalAmt { get; set; }
        public DateTime TransactionDate { get; set; }
        public string EditedBy { get; set; }
        public DateTime EditedDate { get; set; }
        public int Deleted { get; set; }
        public string DeletedBy { get; set; }
        public DateTime DeletedDate { get; set; }

    }
}
