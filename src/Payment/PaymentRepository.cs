using Microsoft.Extensions.Logging;

namespace Payment
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ILogger<PaymentRepository> _logger;
        private static List<Payment> _payments = new();

        public PaymentRepository(ILogger<PaymentRepository> logger)
        {
            _logger = logger;
        }

        public void AddPayment(Payment payment)
        {
            payment.Id = _payments.Count + 1;
            _payments.Add(payment);
        }

        public Payment GetPayment(int paymentId) => _payments.FirstOrDefault(p => p.Id == paymentId);
    }
}
