namespace OrderManagement
{
    public interface ITransactionRepository
    {
        void AddTransaction(Transaction transaction);
        List<Transaction> GetOrderTransactions(int orderId);
    }
}
