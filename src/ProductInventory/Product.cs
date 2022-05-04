namespace ProductInventory
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Default";
        public decimal PricePerUnit { get; set; }
        public string SKU { get; set; } = "00000000";
        public int AvailableQuantity { get; set; }
    }
}
