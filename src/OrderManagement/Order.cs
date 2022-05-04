namespace OrderManagement
{
    public class Order
    {
        public int Id { get; set; }
        public decimal CreditAmount
        {
            get => GetCreditAmount();
        }
        public decimal CreditRequired { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductId { get; set; }
        public List<Transaction> Transactions { get; set; } = new();
        public OrderStatus OrderStatus { get; set; }

        private decimal GetCreditAmount()
        {
            var totalPaymentAmount = Transactions?.Where(t => t.TransactionType == TransactionType.CustomerPayment)?.Sum(t => t.Amount) ?? 0;
            var totalProductCost = Transactions?.Where(t => t.TransactionType == TransactionType.ProductCost)?.Sum(t => t.Amount) ?? 0;
            var totalReturnedAmount = Transactions?.Where(t => t.TransactionType == TransactionType.CustomerBalanceReturn)?.Sum(t => t.Amount) ?? 0;
            return totalPaymentAmount - totalProductCost - totalReturnedAmount;
        }
    }

    public enum OrderStatus
    {
        AwaitingProduct = 10,
        PaymentFailed = 20,
        ProductShipped = 30,
        InsufficientCreditAmount = 40,
        ProductOutOfStock = 50,
        Recalled = 100
    }
}
