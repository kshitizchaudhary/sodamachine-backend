using Microsoft.Extensions.Logging;

namespace ProductInventory
{
    public class ProductRepository : IProductRepository
    {
        private readonly ILogger<ProductRepository> _logger;
        private static List<Product> _products = new()
        {
            new Product { Id = 1, Name = "coke", AvailableQuantity = 5, PricePerUnit = 20, SKU = "ABZZ2345" },
            new Product { Id = 2, Name = "sprite", AvailableQuantity = 3, PricePerUnit = 15, SKU = "CC342345" },
            new Product { Id = 3, Name = "fanta", AvailableQuantity = 3, PricePerUnit = 15, SKU = "PM6F2345" }
        };

        public ProductRepository(ILogger<ProductRepository> logger)
        {
            _logger = logger;
        }

        public Product GetProduct(int productId) => _products.FirstOrDefault(p => p.Id == productId);

        public Product GetProduct(string productName) => _products.FirstOrDefault(p => p.Name.ToLower() == productName.ToLower());

        public List<Product> GetProducts() => _products;
    }
}
