namespace BootCamp.Inventory
{
    using BootCamp.Inventory.Services.Models.Data;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IInventory
    {
        void AddStock(string itemId, int quantity);
        Task<bool> RemoveStockAsync(string itemId, int quantityDesired);
        Task<bool> IsItemInInventory(string itemId);
        Task<bool> CreateItemAsync(InventoryItem item);
        Task<InventoryItem> GetItemAsync(string itemId);

        Task<List<InventoryItem>> GetStoreAsync();
    }
}
