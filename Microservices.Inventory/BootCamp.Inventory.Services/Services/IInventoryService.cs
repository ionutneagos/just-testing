namespace BootCamp.Inventory.Services
{
    using BootCamp.Inventory.Services.Models.Data;
    using BootCamp.Inventory.Services.Models.Request;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IInventoryService
    {
        void AddStock(string itemId);
        Task<bool> RemoveStockAsync(string itemId);
        Task<bool> IsItemInInventory(string itemId);
        Task<bool> CreateItemAsync(CreateInventoryItemRequest item);
        Task<InventoryItem> GetItemAsync(string itemId);

        Task<IList<InventoryItem>> GetStoreAsync();
    }
}
