namespace OrderManagementAPI.Models.Requests
{
    public class AddCreditToOrderRequest
    {
        public int OrderId { get; set; }
        public string PaymentType { get; set; }
        public decimal CreditAmount { get; set; }
    }
}
