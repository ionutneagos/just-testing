namespace BootCamp.ShoppingCart.Services.Models.Data
{
    using Microservices.Shared;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ShoppingCart : Entity
    {
        public string UserId { get; private set; }

        [JsonProperty(PropertyName = "items")]
        private readonly List<ShoppingCartItem> Items;

        public DateTime DateUpdated { get; private set; }

        public ShoppingCart(string userId)
        {
            UserId = userId;
            Items = new List<ShoppingCartItem>();
        }

        public void AddOrUpdateItem(string itemId, decimal unitPrice, int quantity = 1)
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
        }

        public void RemoveItem(string id)
        {
            var item = Items.FirstOrDefault(x => x.Id.Equals(id, StringComparison.InvariantCultureIgnoreCase));
            if (null != item)
            {
                DateUpdated = DateTime.UtcNow;
                Items.Remove(item);
            }
        }

        public int GetItemCount()
        {
            return Items.Select(item => item.Quantity).Aggregate((sum, next) => sum + next);
        }
    }
}