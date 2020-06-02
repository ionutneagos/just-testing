namespace BootCamp.ShoppingCart.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ShoppingCartService : IShoppingCartService
    {
        public Task AddItemAsync(string itemId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<string>> GetItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task RemoveItemAsync(string itemId)
        {
            throw new NotImplementedException();
        }
    }
}
