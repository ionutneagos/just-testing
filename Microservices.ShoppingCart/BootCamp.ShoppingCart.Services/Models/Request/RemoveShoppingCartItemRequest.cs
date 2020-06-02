
namespace BootCamp.ShoppingCart.Services.Models.Request
{
    using System.ComponentModel.DataAnnotations;

    public class RemoveShoppingCartItemRequest
    {
        [Required]
        public string CartId { get; set; }
        [Required]
        public string Id { get; set; }
    }
}
