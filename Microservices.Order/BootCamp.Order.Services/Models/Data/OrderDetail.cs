namespace BootCamp.Order.Services.Models.Data
{
    using Microservices.Shared;
    using Newtonsoft.Json;
    using System;

    [JsonObject(MemberSerialization = MemberSerialization.OptIn)]
    public class OrderDetail : Entity
    {
        [JsonProperty]
        public string Details { get; set; }
        [JsonProperty]
        public DateTime Timestamp { get; private set; }

        public OrderDetail(string details, string id = null)
        {
            Id = id ?? Guid.NewGuid().ToString();
            Details = details;
            Timestamp = DateTime.UtcNow;
        }
    }
}