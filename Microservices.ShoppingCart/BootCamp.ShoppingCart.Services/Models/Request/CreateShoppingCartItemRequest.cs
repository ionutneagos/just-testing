namespace BootCamp.ShoppingCart.Services.Models.Request
{
    using System;
    using System.ComponentModel.DataAnnotations;
    public class CreateShoppingCartItemRequest
    {
        public string CartId { get; set; }
        public string UserId { get; set; }
        [Required]
        public decimal UnitPrice { get; set; }
        [Range(0, Int32.MaxValue, ErrorMessage = "Quantity value must be a positive integer.")]
        public int Quantity { get; set; } = 1;
    }
}
