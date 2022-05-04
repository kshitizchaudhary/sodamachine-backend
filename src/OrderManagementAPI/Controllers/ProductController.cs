using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderManagementAPI.Models.DataContracts;
using ProductInventory;
using System.Net;

namespace OrderManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;
        public ProductsController(ILogger<ProductsController> logger, IProductService productService)
        {
            _productService = productService;
            _logger = logger;   
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<ProductItem>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public IActionResult GetProducts()
        {
            var products = _productService.GetProducts();
            if (products == null || !products.Any())
            {
                var msg = "No product found";
                _logger.LogError(msg);
                return NotFound(new ErrorResponse
                {
                    Message = msg,
                    ErrorCode = ErrorCode.ProductNotFound
                });
            }

            var productItems = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductItem>())).Map<List<Product>, List<ProductItem>>(products);
            return Ok(productItems);
        }

        [HttpGet]
        [Route("{productId}")]
        [ProducesResponseType(typeof(ProductItem), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public IActionResult GetProduct(int productId)
        {
            var product = _productService.GetProduct(productId);
            if (product == null)
            {
                var msg = $"Not product found with Id {productId}.";
                _logger.LogError(msg);
                return NotFound(new ErrorResponse
                {
                    Message = msg,
                    ErrorCode = ErrorCode.ProductNotFound
                });
            }

            var productItem = new Mapper(new MapperConfiguration(cfg => cfg.CreateMap<Product, ProductItem>())).Map<Product, ProductItem>(product);
            return Ok(productItem);
        }
    }
}
