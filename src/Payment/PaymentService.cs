using Microsoft.Extensions.Logging;
using Payment.Exceptions;

namespace Payment
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly ILogger<PaymentService> _logger;

        public PaymentService(ILogger<PaymentService> logger, IPaymentRepository paymentRepository)
        {
            _paymentRepository = paymentRepository;
            _logger = logger;
        }

        public Payment CreditPayment(PaymentType paymentType, decimal amount)
        {
            bool paymentAuthorized = true; // Assuming always authorized
            Payment payment;

            if (!paymentAuthorized)
            {
                var exception = new PaymentUnauthorizedException(paymentType.ToString(), amount);
                _logger.LogError(exception.Message);
                throw exception;
            }

            payment = AddPayment(paymentType, amount, true);
            _logger.LogInformation($"Payment authorized and added. Id={payment.Id} Payment Type={paymentType}, Amount={amount}");
            return payment;
        }

        public Payment DebitPayment(int paymentId, decimal? amount = null)
        {
            var payment = GetPayment(paymentId);

            if (payment == null)
            {
                var exception = new PaymentNotFoundException(paymentId);
                _logger.LogError(exception.Message);
                throw exception;
            }

            var debitAmount = amount ?? payment.Amount;
            var debitPayment = AddPayment(payment.PaymentType, debitAmount * -1, true, paymentId);
            return debitPayment;
        }

        private Payment AddPayment(PaymentType paymentType, decimal amount, bool isAuthorized, int? parentPaymentId = null)
        {
            var payment = new Payment
            {
                Amount = amount,
                PaymentDate = DateTime.Now,
                PaymentType = paymentType,
                IsAuthorized = isAuthorized,
                ParentPaymentId = parentPaymentId
            };
            _paymentRepository.AddPayment(payment);
            return payment;
        }


        public Payment GetPayment(int paymentId) => _paymentRepository.GetPayment(paymentId);
    }
}
