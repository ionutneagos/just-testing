namespace BootCamp.ShoppingCart.Api
{
    using BootCamp.ShoppingCart.Services.Models.Request;
    using Microservices.Shared;
    using Microservices.Shared.Exceptions;
    using Microservices.Shared.Extensions;
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

        [FunctionName("ShoppingCartGet")]
        public static async Task<IActionResult> ShoppingCartGet([HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/{userId}/shoppingCart")] HttpRequest req, [DurableClient] IDurableClient client, ILogger log)
        {
            try
            {
                if (!req.Query.ContainsKey("userId")) 
                    throw new AppException(400, "User Id is missing from query");

                var userId = req.Query["userId"];
                log.LogInformation($"Get Basket for user is called, user Id: {userId}");
               
                var catalogEntity = new EntityId(nameof(ShoppingCart), userId);
                var response = await client.ReadEntityStateAsync<IShoppingCart>(catalogEntity);

                return new OkObjectResult(response.EntityState.GetItemsAsync());

            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }

        [FunctionName("ShoppingCartPost")]
        public static async Task<IActionResult> ShoppingCartPost([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "user/{userId}/shoppingCart/{itemId}")] HttpRequest req, [DurableClient] IDurableClient client, ILogger log)
        {
            try
            {
                log.LogInformation("Add Item To cart is called");

                if (!req.Query.ContainsKey("userId"))
                    throw new AppException(400, "User Id is missing from query");

                var request = await req.GetBodyAsync<CreateShoppingCartItemRequest>();

                await client.SignalEntityAsync<IShoppingCart>(req.Query["userId"], x => x.AddOrUpdateItemAsync(request.ItemId, request.UnitPrice, request.Quantity));

                return new OkResult();

            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }

        [FunctionName("ShoppingCartDelete")]
        public static async Task<IActionResult> ShoppingCartDelete([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "user/{userId}/shoppingCart/{itemId}")] HttpRequest req, [DurableClient] IDurableClient client, ILogger log)
        {
            try
            {
                if (!req.Query.ContainsKey("userId"))
                    throw new AppException(400, "User Id is missing from query");

                if (!req.Query.ContainsKey("itemId")) 
                    throw new AppException(400, "Item Id is missing from query");

                log.LogInformation($"Delete Shopping Cart  is called, basket Id: {req.Query["itemId"]}");

                await client.SignalEntityAsync<IShoppingCart>(req.Query["userId"], x => x.RemoveItemAsync(req.Query["itemId"]));

                return new OkResult();
            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }
    }
}
