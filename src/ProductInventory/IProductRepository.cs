namespace ProductInventory
{
    public interface IProductRepository
    {
        Product GetProduct(int productId);
        Product GetProduct(string productName);
        List<Product> GetProducts();
    }
}