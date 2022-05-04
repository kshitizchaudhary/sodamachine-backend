namespace OrderManagement
{
    public interface IOrderService
    {
        Order CreateOrder(decimal creditAmount);
        bool AddProduct(Order order, int productId);
        void ReturnCredit(Order order);
        Order GetOrder(int orderId);
        void AddCredit(Order order, decimal creditAmount);
    }
}
