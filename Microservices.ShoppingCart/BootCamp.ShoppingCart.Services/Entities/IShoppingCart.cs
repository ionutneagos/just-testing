namespace BootCamp.ShoppingCart
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IShoppingCart
    {
        Task AddOrUpdateItemAsync(string itemId, decimal unitPrice, int quantity = 1);
        Task RemoveItemAsync(string itemId);
        Task<IEnumerable<string>> GetItemsAsync();
    }
}
