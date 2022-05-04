namespace ProductInventory
{
    public interface IProductService
    {
        Product GetProduct(int productId);
        Product GetProduct(string productName);
        List<Product> GetProducts();
        bool ShipProduct(Product product, int noOfUnits = 1);
    }
}
