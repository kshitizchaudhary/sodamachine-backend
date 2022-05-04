using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrderManagement;
using Payment;
using ProductInventory;
using System.Collections.Generic;
using System.Linq;
using UnitTests.TestRepositories;

namespace UnitTests
{
    public abstract class BaseTest
    {
        protected Mock<IProductRepository> ProductRepositoryMock;
        protected Mock<ILogger<ProductService>> ProductServiceLoggerMock;
        protected Mock<IPaymentRepository> PaymentRepositoryMock;
        protected Mock<ILogger<PaymentService>> PaymentServiceLoggerMock;
        protected Mock<IOrderRepository> OrderRepositoryMock;
        protected Mock<ILogger<OrderService>> OrderServiceLoggerMock;

        protected readonly IProductService ProductService;
        protected readonly IPaymentService PaymentService;
        protected readonly IOrderService OrderService;

        private readonly static List<Product> _products = new()
        {
            new Product { Id = 1, Name = "coke", AvailableQuantity = 5, PricePerUnit = 20, SKU = "ABZZ2345" },
            new Product { Id = 2, Name = "sprite", AvailableQuantity = 1, PricePerUnit = 15, SKU = "CC342345" },
            new Product { Id = 3, Name = "fanta", AvailableQuantity = 1, PricePerUnit = 15, SKU = "PM6F2345" }
        };

        protected BaseTest()
        {
            ProductRepositoryMock = new();
            ProductServiceLoggerMock = new();

            ProductRepositoryMock.Setup(x => x.GetProducts()).Returns(_products);
            ProductRepositoryMock.Setup(x => x.GetProduct(1)).Returns(_products.FirstOrDefault(x => x.Id == 1));
            ProductRepositoryMock.Setup(x => x.GetProduct(2)).Returns(_products.FirstOrDefault(x => x.Id == 2));
            ProductRepositoryMock.Setup(x => x.GetProduct(3)).Returns(_products.FirstOrDefault(x => x.Id == 3));
            ProductRepositoryMock.Setup(x => x.GetProduct("coke")).Returns(_products.FirstOrDefault(x => x.Name == "coke"));

            PaymentRepositoryMock = new();
            PaymentServiceLoggerMock = new();

            OrderRepositoryMock = new();
            OrderServiceLoggerMock = new();

            ProductService = new ProductService(ProductServiceLoggerMock.Object, ProductRepositoryMock.Object);
            PaymentService = new PaymentService(PaymentServiceLoggerMock.Object, new PaymentTestRepository());
            OrderService = new OrderService(OrderServiceLoggerMock.Object, new OrderTestRepository(), ProductService, PaymentService);
        }

        [TestCleanup]
        public void RunCleanUp()
        {
            ProductRepositoryMock.Reset();
            ProductServiceLoggerMock.Reset();
            PaymentRepositoryMock.Reset();
            PaymentServiceLoggerMock.Reset();
            OrderRepositoryMock.Reset();
            OrderServiceLoggerMock.Reset();
        }
    }
}