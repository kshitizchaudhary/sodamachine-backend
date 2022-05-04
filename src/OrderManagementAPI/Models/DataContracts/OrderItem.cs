namespace OrderManagementAPI.Models.DataContracts
{
    public class OrderItem
    {
        public int Id { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal CreditRequired { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductId { get; set; }
        public OrderStatus OrderStatus { get; set; }
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

    /*
     * Order Credit -> Order.PaymentFailed
		            Order.ReadyToAddProduct
Add Product -> Order.ProductShipped - (Completed)
		InsufficientBalance 
		OutOfStock
Recall Order -> Order.Recalled
     */
}
