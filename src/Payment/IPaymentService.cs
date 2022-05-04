namespace Payment
{
    public interface IPaymentService
    {
        // Adds new payment and returns new payment Id if payment is authorized
        Payment CreditPayment(PaymentType paymentType, decimal amount);

        // Reverses an existing payment or return payment balance
        Payment DebitPayment(int paymentId, decimal? amount = null);

        Payment GetPayment(int paymentId);
    }
}
