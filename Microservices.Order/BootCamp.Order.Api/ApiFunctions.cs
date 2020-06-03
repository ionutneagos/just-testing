namespace BootCamp.Order.Api
{
    using BootCamp.Order.Services;
    using Microservices.Shared;
    using Microservices.Shared.Exceptions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class ApiFunctions
    {
        private readonly IOrderService orderService;

        public ApiFunctions(IOrderService orderService)
        {
            this.orderService = orderService;
        }

        [FunctionName("OrderGet")]
        public async Task<IActionResult> OrderGet([HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/{userId}/order/{orderId}")] HttpRequest req, ILogger log)
        {
            try
            {
                log.LogInformation($"Order Get is called");

                if (!req.Query.ContainsKey("userId"))
                    throw new AppException(400, "User Id is missing from query");

                if (!req.Query.ContainsKey("orderId"))
                    throw new AppException(400, "Order Id is missing from query");

                return new OkObjectResult(await orderService.GetOrderAsync(req.Query["userId"], req.Query["orderId"]));
            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }

        [FunctionName("OrderCheckoutPost")]
        public async Task<IActionResult> OrderCheckoutPost([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "user/{userId}/order/checkout")] HttpRequest req, ILogger log)
        {
            try
            {
                log.LogInformation("OrderCheckoutPost is called");
                if (!req.Query.ContainsKey("userId"))
                    throw new AppException(400, "User Id is missing from query");

                await orderService.OrderCheckout(req.Query["userId"];

                return new OkResult();
            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }
    }
}
