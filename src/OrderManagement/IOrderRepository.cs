namespace OrderManagement
{
    public interface IOrderRepository
    {
        Order GetOrder(int orderId);
        void CreateOrder(Order order);
        bool UpdateOrder(Order order);
    }
}