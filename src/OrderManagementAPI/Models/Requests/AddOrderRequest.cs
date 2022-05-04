namespace OrderManagementAPI.Models.Requests
{
    public class AddOrderRequest
    {
        public string PaymentType { get; set; }
        public decimal CreditAmount { get; set; }
    }
}
