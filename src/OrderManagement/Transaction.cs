namespace OrderManagement
{
    public class Transaction
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public TransactionType TransactionType { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransactionDate { get; set; }
        public int? PaymentId { get; set; }
    }

    public enum TransactionType
    {
        CustomerPayment = 10,
        ProductCost = 20,
        CustomerBalanceReturn = 30
    }
}
