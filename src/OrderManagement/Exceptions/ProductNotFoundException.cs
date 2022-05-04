namespace OrderManagement.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        public int ProductId { get; private set; }
        public ProductNotFoundException(int productId)
        {
            ProductId = productId;
        }

        public override string Message => $"Product not found with Id {ProductId}.";
    }
}
