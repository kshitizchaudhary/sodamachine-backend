namespace Payment.Exceptions
{
    public class PaymentNotFoundException : Exception
    {
        public int PaymentId { get; private set; }
        public PaymentNotFoundException(int paymentId)
        {
            PaymentId = paymentId;
        }

        public override string Message => $"Payment not found with Id {PaymentId}.";
    }
}
