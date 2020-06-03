namespace BootCamp.Order.Services
{
    using BootCamp.Order.Services.Models.Data;
    using System;
    using System.Threading.Tasks;

    public interface IOrderService
    {
        Task<bool> AddAsync(OrderItem chirp);

        Task<OrderItem> GetOrderAsync(string userId, string orderId);

        void Remove(DateTime timestamp);

        Task OrderCheckout(string userId);
    }
}
