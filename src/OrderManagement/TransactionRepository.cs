namespace OrderManagement
{
    public class TransactionRepository : ITransactionRepository
    {
        private static readonly List<Transaction> _transactions = new List<Transaction>();

        public void AddTransaction(Transaction transaction)
        {
            transaction.Id = _transactions.Count + 1;
            _transactions.Add(transaction);
        }

        /// <summary>
        /// Gets transaction of an order
        /// </summary>
        /// <param name="orderId">order id</param>
        /// <returns>Transactions List</returns>
        public List<Transaction> GetOrderTransactions(int orderId) => _transactions.Where(t => t.OrderId == orderId).ToList();

    }
}
