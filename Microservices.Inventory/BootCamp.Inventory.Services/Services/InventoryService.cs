namespace BootCamp.Inventory.Services
{
    using BootCamp.Inventory.Services.Models.Data;
    using BootCamp.Inventory.Services.Models.Request;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class InventoryService : IInventoryService
    {
        public void AddStock(string itemId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateItemAsync(InventoryItem item)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateItemAsync(CreateInventoryItemRequest item)
        {
            throw new NotImplementedException();
        }

        public Task<InventoryItem> GetItemAsync(string itemId)
        {
            throw new NotImplementedException();
        }

        public Task<IList<InventoryItem>> GetStoreAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsItemInInventory(string itemId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> RemoveStockAsync(string itemId)
        {
            throw new NotImplementedException();
        }
    }
}
