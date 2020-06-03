namespace BootCamp.Order
{
    using BootCamp.Order.Services.Models.Data;
    using System;
    using System.Threading.Tasks;

    public interface IOrder
    {
        Task<bool> AddAsync(OrderDetail chirp);
        Task<OrderDetail> OrderGet(string orderId);
        void Remove(DateTime timestamp);

        Task<bool> Checkout();
    }
}
