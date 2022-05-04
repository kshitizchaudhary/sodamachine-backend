using Microsoft.VisualStudio.TestTools.UnitTesting;
using OrderManagement;

namespace UnitTests
{
    [TestClass]
    public class OrderManagementTests : BaseTest
    {
        [TestMethod]
        public void CreateOrderTest()
        {
            var orderAmount = 20;

            var order = OrderService.CreateOrder(orderAmount);
            Assert.IsNotNull(order);
            Assert.AreEqual(OrderStatus.AwaitingProduct, order.OrderStatus);
            Assert.AreEqual(orderAmount, order.CreditAmount);
        }

        [TestMethod]
        public void AddProductTest()
        {
            var orderAmount = 20;
            var productId = 1;

            var order = OrderService.CreateOrder(orderAmount);
            Assert.IsNotNull(order);
            Assert.AreEqual(OrderStatus.AwaitingProduct, order.OrderStatus);
            Assert.AreEqual(orderAmount, order.CreditAmount);

            OrderService.AddProduct(order, productId);
            Assert.AreEqual(productId, order.ProductId);
            Assert.AreEqual(OrderStatus.ProductShipped, order.OrderStatus);            
        }

        [TestMethod]
        public void RecallOrderTest()
        {
            var orderAmount = 20;

            var order = OrderService.CreateOrder(orderAmount);
            Assert.IsNotNull(order);
            Assert.AreEqual(OrderStatus.AwaitingProduct, order.OrderStatus);
            Assert.AreEqual(orderAmount, order.CreditAmount);

            OrderService.ReturnCredit(order);
            Assert.AreEqual(OrderStatus.Recalled, order.OrderStatus);
            Assert.AreEqual(0, order.CreditAmount);
        }

        [TestMethod]
        public void AddProduct_NotSufficientCredit_Test()
        {
            var orderAmount = 5;
            var productId = 1;

            var order = OrderService.CreateOrder(orderAmount);
            Assert.IsNotNull(order);
            Assert.AreEqual(OrderStatus.AwaitingProduct, order.OrderStatus);
            Assert.AreEqual(orderAmount, order.CreditAmount);

            OrderService.AddProduct(order, productId);
            Assert.AreEqual(0, order.ProductId);
            Assert.AreEqual(OrderStatus.InsufficientCreditAmount, order.OrderStatus);
            Assert.AreEqual(15, order.CreditRequired);
        }

        [TestMethod]
        public void AddProduct_OutOfStock_Test()
        {
            var orderAmount = 15;
            var productId = 2; // Product with 1 available quantity

            var order = OrderService.CreateOrder(orderAmount);
            Assert.IsNotNull(order);
            Assert.AreEqual(OrderStatus.AwaitingProduct, order.OrderStatus);
            Assert.AreEqual(orderAmount, order.CreditAmount);

            OrderService.AddProduct(order, productId);
            Assert.AreEqual(productId, order.ProductId);
            Assert.AreEqual(OrderStatus.ProductShipped, order.OrderStatus);

            var nextOrder = OrderService.CreateOrder(orderAmount);
            Assert.IsNotNull(nextOrder);
            Assert.AreEqual(OrderStatus.AwaitingProduct, nextOrder.OrderStatus);
            Assert.AreEqual(orderAmount, nextOrder.CreditAmount);

            OrderService.AddProduct(nextOrder, productId);
            Assert.AreEqual(0, nextOrder.ProductId);
            Assert.AreEqual(OrderStatus.ProductOutOfStock, nextOrder.OrderStatus);
        }

        [TestMethod]
        public void AddProduct_OutOfStock_RecallOrder_Test()
        {
            var orderAmount = 15;
            var productId = 3; // Product with 1 available quantity

            var order = OrderService.CreateOrder(orderAmount);
            Assert.IsNotNull(order);
            Assert.AreEqual(OrderStatus.AwaitingProduct, order.OrderStatus);
            Assert.AreEqual(orderAmount, order.CreditAmount);

            OrderService.AddProduct(order, productId);
            Assert.AreEqual(productId, order.ProductId);
            Assert.AreEqual(OrderStatus.ProductShipped, order.OrderStatus);

            var nextOrder = OrderService.CreateOrder(orderAmount);
            Assert.IsNotNull(nextOrder);
            Assert.AreEqual(OrderStatus.AwaitingProduct, nextOrder.OrderStatus);
            Assert.AreEqual(orderAmount, nextOrder.CreditAmount);

            OrderService.AddProduct(nextOrder, productId);
            Assert.AreEqual(0, nextOrder.ProductId);
            Assert.AreEqual(OrderStatus.ProductOutOfStock, nextOrder.OrderStatus);

            OrderService.ReturnCredit(nextOrder);
            Assert.AreEqual(0, nextOrder.ProductId);
            Assert.AreEqual(OrderStatus.Recalled, nextOrder.OrderStatus);
        }

        [TestMethod]
        public void AddProduct_NotSufficientCredit_AddMoreCredit_Test()
        {
            var orderAmount = 5;
            var creditRequired = 15;
            var productId = 1;

            var order = OrderService.CreateOrder(orderAmount);
            Assert.IsNotNull(order);
            Assert.AreEqual(OrderStatus.AwaitingProduct, order.OrderStatus);
            Assert.AreEqual(orderAmount, order.CreditAmount);

            OrderService.AddProduct(order, productId);
            Assert.AreEqual(0, order.ProductId);
            Assert.AreEqual(OrderStatus.InsufficientCreditAmount, order.OrderStatus);
            Assert.AreEqual(creditRequired, order.CreditRequired);

            OrderService.AddCredit(order, order.CreditRequired);
            Assert.AreEqual(orderAmount + creditRequired, order.CreditAmount);

            OrderService.AddProduct(order, productId);
            Assert.AreEqual(productId, order.ProductId);
            Assert.AreEqual(OrderStatus.ProductShipped, order.OrderStatus);
        }

        [TestMethod]
        public void AddProduct_NotSufficientCredit_RecallOrder_Test()
        {
            var orderAmount = 5;
            var creditRequired = 15;
            var productId = 1;

            var order = OrderService.CreateOrder(orderAmount);
            Assert.IsNotNull(order);
            Assert.AreEqual(OrderStatus.AwaitingProduct, order.OrderStatus);
            Assert.AreEqual(orderAmount, order.CreditAmount);

            OrderService.AddProduct(order, productId);
            Assert.AreEqual(0, order.ProductId);
            Assert.AreEqual(OrderStatus.InsufficientCreditAmount, order.OrderStatus);
            Assert.AreEqual(creditRequired, order.CreditRequired);

            OrderService.AddCredit(order, order.CreditRequired);
            Assert.AreEqual(orderAmount + creditRequired, order.CreditAmount);

            OrderService.ReturnCredit(order);
            Assert.AreEqual(0, order.ProductId);
            Assert.AreEqual(OrderStatus.Recalled, order.OrderStatus);
        }
    }
}