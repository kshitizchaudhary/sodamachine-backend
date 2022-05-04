namespace Payment
{
    public interface IPaymentRepository
    {
        void AddPayment(Payment payment);
        Payment GetPayment(int paymentId);
    }
}
