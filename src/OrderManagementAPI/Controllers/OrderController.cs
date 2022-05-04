using Microsoft.AspNetCore.Mvc;
using OrderManagement;
using OrderManagement.Exceptions;
using OrderManagementAPI.Models.DataContracts;
using OrderManagementAPI.Models.Requests;
using OrderManagementAPI.Models.Responses;
using Payment.Exceptions;
using System.Net;
using DataContractOrderStatus = OrderManagementAPI.Models.DataContracts.OrderStatus;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(ILogger<OrderController> logger, IOrderService orderService)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType(typeof(OrderItem), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(PaymentErrorResponse), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public IActionResult AddOrder(AddOrderRequest request)
        {
            try
            {
                var order = _orderService.CreateOrder(request.CreditAmount);
                if (order == null)
                {
                    return StatusCode(500, new ErrorResponse
                    {
                        Message = "Not able to create an order.",
                        ErrorCode = ErrorCode.NotAbleToCreateOrder
                    });
                }

                return CreatedAtAction(
                    nameof(GetOrder),
                    new { orderId = order.Id },
                    new OrderItem
                    {
                        Id = order.Id,
                        CreditAmount = order.CreditAmount,
                        OrderDate = order.OrderDate,
                        OrderStatus = (DataContractOrderStatus)order.OrderStatus
                    });
            }
            catch (PaymentUnauthorizedException ex)
            {
                return BadRequest(new PaymentErrorResponse
                {
                    Message = ex.Message,
                    ErrorCode = ErrorCode.PaymentUnauthorized,
                    PaymentAmount = request.CreditAmount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ErrorResponse
                {
                    Message = ex.Message,
                    ErrorCode = ErrorCode.UnhandledException
                });
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(OrderItem), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public IActionResult AddCreditToOrder(AddCreditToOrderRequest request)
        {
            try
            {
                var order = _orderService.GetOrder(request.OrderId);
                if (order == null)
                {
                    var msg = $"Order does not exists with Id {request.OrderId}";
                    _logger.LogError(msg);
                    return NotFound(new ErrorResponse
                    {
                        Message = msg,
                        ErrorCode = ErrorCode.OrderNotFound
                    });
                }

                _orderService.AddCredit(order, request.CreditAmount);

                return Ok(
                    new OrderItem
                    {
                        Id = order.Id,
                        CreditAmount = order.CreditAmount,
                        OrderDate = order.OrderDate,
                        OrderStatus = (DataContractOrderStatus)order.OrderStatus
                    });
            }
            catch (PaymentUnauthorizedException ex)
            {
                _logger.LogError(ex.Message);
                return BadRequest(new PaymentErrorResponse
                {
                    Message = ex.Message,
                    ErrorCode = ErrorCode.PaymentUnauthorized,
                    PaymentAmount = request.CreditAmount
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorResponse
                {
                    Message = ex.Message,
                    ErrorCode = ErrorCode.UnhandledException
                });
            }
        }

        [HttpPut]
        [Route("{orderId}/recall")]
        [ProducesResponseType(typeof(OrderItem), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public IActionResult RecallOrder(int orderId)
        {
            try
            {
                var order = _orderService.GetOrder(orderId);
                if (order == null)
                {
                    var msg = $"Order does not exists with Id {orderId}";
                    _logger.LogError(msg);
                    return NotFound(new ErrorResponse
                    {
                        Message = msg,
                        ErrorCode = ErrorCode.OrderNotFound
                    });
                }

                _orderService.ReturnCredit(order);

                return Ok(
                    new OrderItem
                    {
                        Id = order.Id,
                        CreditAmount = order.CreditAmount,
                        OrderDate = order.OrderDate,
                        OrderStatus = (DataContractOrderStatus)order.OrderStatus
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorResponse
                {
                    Message = ex.Message,
                    ErrorCode = ErrorCode.UnhandledException
                });
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(OrderItem), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public IActionResult GetOrder(int orderId)
        {
            var order = _orderService.GetOrder(orderId);
            if (order == null)
            {
                var msg = $"Order does not exists with Id {orderId}";
                _logger.LogError(msg);
                return NotFound(new ErrorResponse
                {
                    Message = msg,
                    ErrorCode = ErrorCode.OrderNotFound
                });
            }

            return Ok(new OrderItem
            {
                Id = order.Id,
                CreditAmount = order.CreditAmount,
                OrderDate = order.OrderDate,
                OrderStatus = (DataContractOrderStatus)order.OrderStatus
            });
        }

        [HttpPost]
        [Route("{orderId}/product/{productId}")]
        [ProducesResponseType(typeof(AddProductResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.InternalServerError)]
        public IActionResult AddProduct(int orderId, int productId)
        {
            try
            {
                var order = _orderService.GetOrder(orderId);
                if (order == null)
                {
                    var msg = $"Order does not exists with Id {orderId}";
                    _logger.LogError(msg);
                    return NotFound(new ErrorResponse
                    {
                        Message = msg,
                        ErrorCode = ErrorCode.OrderNotFound
                    });
                }

                var success = _orderService.AddProduct(order, productId);

                var response = new AddProductResponse
                {
                    Order = new OrderItem
                    {
                        Id = order.Id,
                        CreditAmount = order.CreditAmount,
                        OrderDate = order.OrderDate,
                        OrderStatus = (DataContractOrderStatus)order.OrderStatus,
                        ProductId = success ? productId : 0,
                    },
                    CustomerBalanceReturned = GetCustomerBalanceReturned(order),
                    MissingCreditAmount = GetMissingCreditAmount(order)
                };

                return Ok(response);
            }
            catch (ProductNotFoundException pEx)
            {
                _logger.LogError(pEx.Message);
                return NotFound(new ErrorResponse
                {
                    Message = pEx.Message,
                    ErrorCode = ErrorCode.ProductNotFound
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500, new ErrorResponse
                {
                    Message = ex.Message,
                    ErrorCode = ErrorCode.UnhandledException
                });
            }
        }

        private decimal GetCustomerBalanceReturned(Order order)
        {
            if (order.Transactions.Any())
            {
                var customerBalanceReturnTransaction = order.Transactions.FirstOrDefault(t => t.TransactionType == TransactionType.CustomerBalanceReturn);
                return customerBalanceReturnTransaction?.Amount ?? 0;
            }

            return 0;
        }

        private decimal GetMissingCreditAmount(Order order) => order.CreditRequired;
    }
}
