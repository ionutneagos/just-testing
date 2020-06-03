namespace BootCamp.Order.Api
{
    using Microservices.Shared;
    using Microservices.Shared.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.DurableTask;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public static class ApiFunctions
    {

        [FunctionName("OrderGetAll")]
        public static async Task<IActionResult> OrderGetAll([HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/{userId}/order")] HttpRequest req, 
            [DurableClient] IDurableClient client, ILogger log)
        {
            try
            {
                log.LogInformation($"Order Get is called");

                if (!req.Query.ContainsKey("userId"))
                    throw new AppException(400, "User Id is missing from query");

                var orderEntity = new EntityId(nameof(Order), req.Query["userId"]);

                var response = await client.ReadEntityStateAsync<Order>(orderEntity);
                if (!response.EntityExists)
                    return new NotFoundResult();

                return new OkObjectResult(response.EntityState.Items);

            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }

        [FunctionName("OrderGet")]
        public static async Task<IActionResult> OrderGet([HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/{userId}/order/{orderId}")] HttpRequest req,
           [DurableClient] IDurableClient client, ILogger log)
        {
            try
            {
                log.LogInformation($"Order Get is called");

                if (!req.Query.ContainsKey("userId"))
                    throw new AppException(400, "User Id is missing from query");

                if (!req.Query.ContainsKey("orderId"))
                    throw new AppException(400, "Order Id is missing from query");
              
                var orderEntity = new EntityId(nameof(Order), req.Query["userId"]);

                var response = await client.ReadEntityStateAsync<IOrder>(orderEntity);
                if (!response.EntityExists)
                    return new NotFoundResult();
                
                var order = response.EntityState.OrderGet(req.Query["orderId"]);
               
                if (order == null)
                    return new NotFoundResult();
                else
                    return new OkObjectResult(order);
            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }


        [FunctionName("OrderCheckoutPost")]
        public static async Task<IActionResult> OrderCheckoutPost([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "user/{userId}/order/checkout")] HttpRequest req, [DurableClient] IDurableClient client, ILogger log)
        {
            try
            {
                log.LogInformation("OrderCheckoutPost is called");
                if (!req.Query.ContainsKey("userId"))
                    throw new AppException(400, "User Id is missing from query");

                await client.SignalEntityAsync<IOrder>(req.Query["userId"], x => x.Checkout());

                return new OkResult();

            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }
    }
}
