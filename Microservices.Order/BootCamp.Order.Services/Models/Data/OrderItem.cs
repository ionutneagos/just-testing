namespace BootCamp.Order.Services.Models.Data
{
    using Microservices.Shared;
    using System;
    using System.Collections.Generic;

    public class OrderItem : Entity
    {
        public string UserId { get; set; }

        public DateTime Timestamp { get; set; }

        public IEnumerable<string> Details { get; set; }
    }
}