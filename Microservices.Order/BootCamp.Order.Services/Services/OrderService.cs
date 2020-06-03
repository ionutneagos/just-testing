namespace BootCamp.Order.Services
{
    using BootCamp.Order.Services.Models.Data;
    using System;
    using System.Threading.Tasks;

    public class OrderService : IOrderService
    {
        public Task<bool> AddAsync(OrderItem chirp)
        {
            throw new NotImplementedException();
        }

        public Task<OrderItem> GetOrderAsync(string userId, string orderId)
        {
            throw new NotImplementedException();
        }

        public Task OrderCheckout(string userId)
        {
            throw new NotImplementedException();
        }

        public void Remove(DateTime timestamp)
        {
            throw new NotImplementedException();
        }
    }
}
