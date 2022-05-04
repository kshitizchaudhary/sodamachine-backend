using Microsoft.Extensions.Logging;

namespace OrderManagement
{
    public class OrderRepository : IOrderRepository
    {
        private static List<Order> _orders = new List<Order>();
        private readonly ILogger<OrderRepository> _logger;

        public OrderRepository(ILogger<OrderRepository> logger)
        {
            _logger = logger;
        }

        public Order GetOrder(int orderId) => _orders.FirstOrDefault(o => o.Id == orderId);

        public void CreateOrder(Order order)
        {
            order.Id = _orders.Count + 1;
            _orders.Add(order);
        }

        /// <summary>
        /// Updates an order
        /// </summary>
        /// <param name="order">Order to update</param>
        /// <returns>True if order is found and update is successful else False</returns>
        public bool UpdateOrder(Order order)
        {
            var orderIndex = _orders.FindIndex(o => o.Id == order.Id);

            if (orderIndex >= 0)
            {
                _orders[orderIndex] = order;
                return true;
            }

            return false;
        }

    }
}
