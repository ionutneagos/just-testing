namespace BootCamp.ShoppingCart.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IShoppingCartService
    {
        Task AddItemAsync(string itemId);
        Task RemoveItemAsync(string itemId);
        Task<IEnumerable<string>> GetItemsAsync();
    }
}
