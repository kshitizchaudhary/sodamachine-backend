using OrderManagement;
using System.Collections.Generic;
using System.Linq;

namespace UnitTests.TestRepositories
{
    public class OrderTestRepository : IOrderRepository
    {
        private static List<Order> _orders = new List<Order>();
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