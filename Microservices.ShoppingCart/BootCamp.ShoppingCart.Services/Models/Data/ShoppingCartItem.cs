
namespace BootCamp.ShoppingCart.Services.Models.Data
{
    using Microservices.Shared;

    public class ShoppingCartItem : Entity
    {
        public string CartId { get; private set; }
        public decimal UnitPrice { get; private set; }
        public int Quantity { get; private set; }

        public ShoppingCartItem(string cartId, string itemId, decimal unitPrice, int quantity)
        {
            Id = itemId;
            CartId = cartId;
            UnitPrice = unitPrice;
            Quantity = quantity;
        }

        public void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }

        public void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
        }


        public override string ToString()
        {
            return string.Format(
                 "Item {0}: {1}, cart id {1}, price {2} quantity {3}.",
                this.Id,
                this.CartId,
                this.UnitPrice.ToString(),
                this.Quantity.ToString());
        }
    }
}