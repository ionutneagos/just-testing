namespace BootCamp.ShoppingCart
{
    using BootCamp.ShoppingCart.Services.Models.Data;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class ShoppingCart : Microservices.Shared.Entity, IShoppingCart
    {
        [JsonProperty(PropertyName = "userId")]
        public string UserId { get; private set; }

        [JsonProperty(PropertyName = "items")]
        private readonly List<ShoppingCartItem> Items;

        [JsonProperty(PropertyName = "dateUpdated")]
        public DateTime DateUpdated { get; private set; }

        [FunctionName(nameof(ShoppingCart))]
        public static Task HandleEntityOperation([EntityTrigger] IDurableEntityContext context)
        {
            return context.DispatchAsync<ShoppingCart>();
        }

        public ShoppingCart(string userId, string id = null)
        {
            Id = id ?? Guid.NewGuid().ToString();
            UserId = userId;
            Items = new List<ShoppingCartItem>();
        }

        public Task AddOrUpdateItemAsync(string itemId, decimal unitPrice, int quantity = 1)
        {
            DateUpdated = DateTime.UtcNow;
            if (quantity == 0)
            {
                Items.RemoveAll(item => item.Id == itemId);
            }
            else
            {
                if (!Items.Any(i => i.Id == itemId))
                {
                    Items.Add(new ShoppingCartItem(Id, itemId, unitPrice, quantity));
                }
                else
                {
                    var existingItemIndex = Items.FindIndex(i => i.Id == itemId);
                    Items[existingItemIndex].UpdateQuantity(quantity);
                }
            }
            return Task.FromResult(true);
        }
       
        public Task RemoveItemAsync(string itemId)
        {
            var item = Items.FirstOrDefault(x => x.Id.Equals(itemId, StringComparison.InvariantCultureIgnoreCase));
            if (null != item)
            {
                DateUpdated = DateTime.UtcNow;
                Items.Remove(item);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<IEnumerable<string>> GetItemsAsync()
        {
            return Task.FromResult(Items.Select(x=>x.ToString()));
        }

        public override string ToString()
        {
            return string.Format(
                 "Item {0}:UserId {1}, items in cart: {2}.",
                this.Id,
                this.UserId,
                this.Items.Select(t => t.ToString()));
        }
    }
}