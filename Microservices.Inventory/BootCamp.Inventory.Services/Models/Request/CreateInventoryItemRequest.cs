namespace BootCamp.Inventory.Services.Models.Request
{
    public class CreateInventoryItemRequest
    {
        public int AvailableStock { get; private set; }
        public decimal Price { get; }
        public string Description { get; }
        public int RestockThreshold { get; }
        public int MaxStockThreshold { get; }
    }
}
