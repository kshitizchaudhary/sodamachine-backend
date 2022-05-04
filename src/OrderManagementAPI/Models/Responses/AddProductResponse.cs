using OrderManagementAPI.Models.DataContracts;

namespace OrderManagementAPI.Models.Responses
{
    public class AddProductResponse
    {
        public OrderItem Order { get; set; }
        public decimal? CustomerBalanceReturned { get; set; }
        public decimal? MissingCreditAmount { get; set; }
    }
}
