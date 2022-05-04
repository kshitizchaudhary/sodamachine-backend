using Microsoft.Extensions.Logging;

namespace ProductInventory
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(ILogger<ProductService> logger, IProductRepository productRepository)
        {
            _productRepository = productRepository;
            _logger = logger;
        }

        public Product GetProduct(int productId) => _productRepository.GetProduct(productId);

        public Product GetProduct(string productName) => _productRepository.GetProduct(productName);

        public List<Product> GetProducts() => _productRepository.GetProducts();

        public bool ShipProduct(Product product, int noOfUnits = 1)
        {
            if (noOfUnits > 0 && product.AvailableQuantity >= noOfUnits)
            {
                product.AvailableQuantity--;
                _logger.LogInformation($"Product Id {product.Id}, SKU {product.SKU} {noOfUnits} item(s) shipped.");
                return true;
            }

            _logger.LogInformation($"Product Id {product.Id}, SKU {product.SKU} No sufficient product inventory to ship {noOfUnits} item(s).");
            return false;
        }
    }
}
