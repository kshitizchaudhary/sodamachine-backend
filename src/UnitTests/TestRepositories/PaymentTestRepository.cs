using Payment;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.TestRepositories
{
    public class PaymentTestRepository : IPaymentRepository
    {
        private static List<Payment.Payment> _payments = new();

        public void AddPayment(Payment.Payment payment)
        {
            payment.Id = _payments.Count + 1;
            _payments.Add(payment);
        }

        public Payment.Payment GetPayment(int paymentId) => _payments.FirstOrDefault(p => p.Id == paymentId);
    }
}
