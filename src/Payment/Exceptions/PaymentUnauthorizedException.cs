namespace Payment.Exceptions
{
    public class PaymentUnauthorizedException : Exception
    {
        public string PaymentType { get; private set; }
        public decimal Amount { get; private set; }
        public PaymentUnauthorizedException(string paymentType, decimal amount)
        {
            PaymentType = paymentType;
            Amount = amount;
        }

        public override string Message => $"Payment not authorized. Payment Type={PaymentType}, Amount={Amount}";
    }
}
