namespace BootCamp.ShoppingCart.Services.Models.Request
{
    public class CreateShoppingCartItemRequest
    {
        public string ItemId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; } = 1;
    }
}
