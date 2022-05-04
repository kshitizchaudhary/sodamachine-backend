using Microsoft.VisualStudio.TestTools.UnitTesting;
using Payment;

namespace UnitTests
{
    [TestClass]
    public class PaymentTests : BaseTest
    {
        [TestMethod]
        public void CreditPaymentTest()
        {
            var payment = PaymentService.CreditPayment(PaymentType.Cash, 500);
            Assert.IsNotNull(payment);
            Assert.AreEqual(500, payment.Amount);

            var committedPayment = PaymentService.GetPayment(payment.Id);
            Assert.IsNotNull(committedPayment);
            Assert.AreEqual(500, payment.Amount);
            Assert.AreEqual(payment.Id, committedPayment.Id);
        }

        [TestMethod]
        public void DebitPayment_FullAmount_Test()
        {
            var payment = PaymentService.CreditPayment(PaymentType.Cash, 500);
            Assert.IsNotNull(payment);
            Assert.AreEqual(500, payment.Amount);

            var debitedPayment = PaymentService.DebitPayment(payment.Id);
            Assert.IsNotNull(debitedPayment);
            Assert.AreEqual(-500, debitedPayment.Amount);
        }

        [TestMethod]
        public void DebitPayment_PartialAmount_Test()
        {
            var payment = PaymentService.CreditPayment(PaymentType.Cash, 500);
            Assert.IsNotNull(payment);
            Assert.AreEqual(500, payment.Amount);

            var debitedPayment = PaymentService.DebitPayment(payment.Id, 200);
            Assert.IsNotNull(debitedPayment);
            Assert.AreEqual(-200, debitedPayment.Amount);
        }
    }
}