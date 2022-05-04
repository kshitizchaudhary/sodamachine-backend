namespace Payment
{
    public class Payment
    {
        public int Id { get; set; }
        public PaymentType PaymentType { get; set; }
        public bool IsAuthorized { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public int? ParentPaymentId { get; set; }
    }

    public enum PaymentType
    {
        Cash = 1,
        BankCard = 2,
        SMS = 3
    }
}