namespace BootCamp.Inventory
{
    using BootCamp.Inventory.Services.Models.Data;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Inventory : IInventory
    {
        [JsonProperty]
        public static string Id = Guid.NewGuid().ToString();
       
        [JsonProperty]
        public List<InventoryItem> Items { get; set; } = new List<InventoryItem>();

        [FunctionName(nameof(Inventory))]
        public static Task HandleEntityOperation([EntityTrigger] IDurableEntityContext context)
        {
            return context.DispatchAsync<Inventory>();
        }

        public void AddStock(string itemId, int quantity)
        {
            var item = GetItemAsync(itemId).Result;
            item.AddStock(quantity);
        }

        public Task<bool> CreateItemAsync(InventoryItem item)
        {
            Items.Add(item);
            return Task.FromResult(true);
        }

        public Task<InventoryItem> GetItemAsync(string itemId)
        {
            return Task.FromResult(Items.FirstOrDefault(x => x.Id == itemId));
        }

        public Task<List<InventoryItem>> GetStoreAsync()
        {
            return Task.FromResult(Items);
        }

        public Task<bool> IsItemInInventory(string itemId)
        {
            return Task.FromResult(Items.Any(x => x.Id == itemId && x.AvailableStock > 0));
        }

        public Task<bool> RemoveStockAsync(string itemId, int quantityDesired)
        {
            var item = GetItemAsync(itemId).Result;
            var result = item.RemoveStock(quantityDesired);
            return Task.FromResult(quantityDesired == result);
        }
    }
}
