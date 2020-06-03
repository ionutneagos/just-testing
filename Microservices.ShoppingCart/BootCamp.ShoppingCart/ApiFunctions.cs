

namespace BootCamp.ShoppingCart.Api
{
    using BootCamp.ShoppingCart.Services;
    using BootCamp.ShoppingCart.Services.Models.Request;
    using Microservices.Shared;
    using Microservices.Shared.Exceptions;
    using Microservices.Shared.Extensions;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Azure.WebJobs;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Threading.Tasks;

    public class ApiFunctions
    {
        private readonly IShoppingCartService cartService;

        public ApiFunctions(IShoppingCartService cartService)
        {
            this.cartService = cartService;
        }

        [FunctionName("ShoppingCartGet")]
        public async Task<IActionResult> ShoppingCartGet([HttpTrigger(AuthorizationLevel.Function, "get", Route = "user/{userId}/shoppingCart")] HttpRequest req, ILogger log)
        {
            try
            {
                if (!req.Query.ContainsKey("userId")) 
                    throw new AppException(400, "User Id is missing from query");

                var userId = req.Query["userId"];

                log.LogInformation($"Get Basket for user is called, user Id: {userId}");

                var basket = await cartService.GetItemsAsync();

                return new OkObjectResult(basket);
            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }

        [FunctionName("ShoppingCartPost")]
        public async Task<IActionResult> ShoppingCartPost([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "user/{userId}/shoppingCart/{itemId}")] HttpRequest req, ILogger log)
        {
            try
            {
                log.LogInformation("Add Item To Basket is called");

                if (!req.Query.ContainsKey("userId"))
                    throw new AppException(400, "User Id is missing from query");

                var request = await req.GetBodyAsync<CreateShoppingCartItemRequest>();
                await cartService.AddItemAsync(request.CartId);
                return new OkObjectResult(request);
            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }

        [FunctionName("ShoppingCartDelete")]
        public async Task<IActionResult> ShoppingCartDelete([HttpTrigger(AuthorizationLevel.Function, "delete", Route = "user/{userId}/shoppingCart/{itemId}")] HttpRequest req, ILogger log)
        {
            try
            {
                if (!req.Query.ContainsKey("userId"))
                    throw new AppException(400, "User Id is missing from query");

                if (!req.Query.ContainsKey("itemId")) throw new AppException(400, "Item Id is missing from query");

                var itemId = req.Query["itemId"];

                log.LogInformation($"Delete ShoppingCartDelete is called, basket Id: {itemId}");

                await cartService.RemoveItemAsync(itemId);

                return new OkResult();
            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }
    }
}
