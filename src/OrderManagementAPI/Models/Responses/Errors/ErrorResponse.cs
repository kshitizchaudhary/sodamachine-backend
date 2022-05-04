namespace OrderManagementAPI.Models.DataContracts
{
    public class ErrorResponse
    {
        public string Message { get; set; }
        public ErrorCode ErrorCode { get; set; }
    }

    public class PaymentErrorResponse : ErrorResponse
    {
        public decimal PaymentAmount { get; set; }
    }

    public enum ErrorCode
    {
        PaymentUnauthorized = 1001,
        NotAbleToCreateOrder = 1002,
        OrderNotFound = 1003,
        ProductNotFound = 1004,
        UnhandledException = 1005
    }
}
