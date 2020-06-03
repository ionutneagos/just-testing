namespace BootCamp.Order
{
    using BootCamp.Order.Services.Models.Data;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class Order : IOrder
    {
        [JsonProperty]
        public string UserId { get; private set; }
      
        [JsonProperty]
        public List<OrderDetail> Items { get; set; } = new List<OrderDetail>();


        [FunctionName(nameof(Order))]
        public static Task HandleEntityOperation([EntityTrigger] IDurableEntityContext context)
        {
            return context.DispatchAsync<Order>();
        }

        public Order(string userId)
        {
            UserId = userId;
            Items = new List<OrderDetail>();
        }

        public Task<bool> AddAsync(OrderDetail chirp)
        {
            Items.Add(new OrderDetail(chirp.Details));
            return Task.FromResult(true);
        }

        public void Remove(DateTime timestamp)
        {
            var item = Items.FirstOrDefault(x => x.Timestamp == timestamp);
            if (null != item)
                Items.Remove(item);
        }
        public Task<OrderDetail> OrderGet(string orderId)
        {
            return Task.FromResult(Items.FirstOrDefault(x => x.Id == orderId));
        }

        public Task<bool> Checkout()
        {
            //do validation
            Items = new List<OrderDetail>();
            return Task.FromResult(true);
        }
    }
}
