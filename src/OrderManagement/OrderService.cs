using Microsoft.Extensions.Logging;
using OrderManagement.Exceptions;
using Payment;
using ProductInventory;

namespace OrderManagement
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductService _productService;
        private readonly IPaymentService _paymentService;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ILogger<OrderService> logger, IOrderRepository orderRepository, IProductService productService, IPaymentService paymentService)
        {
            _orderRepository = orderRepository;
            _productService = productService;
            _paymentService = paymentService;
            _logger = logger;
        }

        public Order CreateOrder(decimal creditAmount)
        {
            // Authorize payment
            var payment = _paymentService.CreditPayment(PaymentType.Cash, creditAmount);

            //TODO: Assuming payment is always authorized
            var order = new Order 
            { 
                OrderDate = DateTime.Now, 
                OrderStatus = OrderStatus.AwaitingProduct, 
                Transactions = new() 
            };
            
            _orderRepository.CreateOrder(order);

            AddPaymentTransaction(order, payment.Id, creditAmount);

            UpdateOrder(order);

            return order;
        }

        public bool AddProduct(Order order, int productId)
        {
            if (order.OrderStatus == OrderStatus.AwaitingProduct || order.OrderStatus == OrderStatus.ProductOutOfStock)
            {
                var product = _productService.GetProduct(productId);

                if (product == null)
                {
                    var exception = new ProductNotFoundException(productId);
                    _logger.LogError(exception.Message);
                    throw exception;
                }
                
                return ProcessOrder(order, product);
            }
            else
            {
                _logger.LogWarning($"Payment not authorized.");
                return false;
            }
        }

        public void ReturnCredit(Order order)
        {
            var paymentTransaction = order.Transactions.FirstOrDefault(t => t.PaymentId.HasValue);

            if (paymentTransaction == null || !paymentTransaction.PaymentId.HasValue)
            {
                var ex = new Exception("No payment transaciton found in the order");
                _logger.LogError(ex.Message);
                throw ex;
            }

            var returnedPayment = _paymentService.DebitPayment(paymentTransaction.PaymentId.Value, order.CreditAmount);

            if (returnedPayment != null)
            {
                AddCustomerBalanceReturnTransaction(order, order.CreditAmount);

                if (!order.Transactions.Any(t => t.TransactionType == TransactionType.ProductCost))
                {
                    order.OrderStatus = OrderStatus.Recalled;                
                }

                UpdateOrder(order);
                return;
            }

            var exception = new Exception("Cannot return customer balance");
            _logger.LogError(exception.Message);
            throw exception;

        }

        private bool ProcessOrder(Order order, Product product)
        {
            // Check product amount with the credit
            var creditBalance = order.CreditAmount - product.PricePerUnit;
            bool success = false;

            if (creditBalance >= 0)
            {
                if (_productService.ShipProduct(product))
                {
                    AddProductCostTransaction(order, product.PricePerUnit);

                    order.ProductId = product.Id;
                    order.OrderStatus = OrderStatus.ProductShipped; // success

                    success = true;
                }
                else
                {
                    order.OrderStatus = OrderStatus.ProductOutOfStock; // Recall or choose another product
                }
            }
            else
            {
                order.OrderStatus = OrderStatus.InsufficientCreditAmount; // Recall or choose another product (with less value)
                order.CreditRequired = creditBalance * -1;
            }

            UpdateOrder(order);

            return success;
        }

        

        public void AddCredit(Order order, decimal creditAmount)
        {
            // Authorize payment
            var payment = _paymentService.CreditPayment(PaymentType.Cash, creditAmount);

            AddPaymentTransaction(order, payment.Id, creditAmount);
            order.OrderStatus = OrderStatus.AwaitingProduct;
            
            UpdateOrder(order);
        }

        private void UpdateOrder(Order order)
        {
            var updated = _orderRepository.UpdateOrder(order);

            if (!updated)
            {
                var exception = new Exception("Order not updated sucessfully");
                _logger.LogError(exception.Message);
                throw exception;
            }
        }

        private void AddPaymentTransaction(Order order, int paymentId, decimal paidAmount)
        {
            var transaction = new Transaction
            {
                TransactionType = TransactionType.CustomerPayment,
                Amount = paidAmount,
                TransactionDate = DateTime.Now,
                OrderId = order.Id,
                PaymentId = paymentId
            };

            new TransactionRepository().AddTransaction(transaction);

            order.Transactions.Add(transaction);
        }

        private void AddProductCostTransaction(Order order, decimal productCost)
        {
            var transaction = new Transaction
            {
                TransactionType = TransactionType.ProductCost,
                Amount = productCost,
                TransactionDate = DateTime.Now,
                OrderId = order.Id
            };

            new TransactionRepository().AddTransaction(transaction);

            order.Transactions.Add(transaction);
        }

        private void AddCustomerBalanceReturnTransaction(Order order, decimal amountReturned)
        {
            var transaction = new Transaction
            {
                TransactionType = TransactionType.CustomerBalanceReturn,
                Amount = amountReturned,
                TransactionDate = DateTime.Now,
                OrderId = order.Id
            };

            new TransactionRepository().AddTransaction(transaction);

            order.Transactions.Add(transaction);
        }

        public Order GetOrder(int orderId) => _orderRepository.GetOrder(orderId);
    }
}
