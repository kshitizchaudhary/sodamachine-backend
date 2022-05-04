namespace OrderManagementAPI.Models.DataContracts
{
    public class ProductItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal PricePerUnit { get; set; }
        public string SKU { get; set; }
        public int AvailableQuantity { get; set; }
    }
}
