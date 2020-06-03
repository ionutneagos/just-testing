

namespace BootCamp.Inventory.Api
{
    using BootCamp.Inventory.Services;
    using BootCamp.Inventory.Services.Models.Request;
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
        private readonly IInventoryService inventoryService;

        public ApiFunctions(IInventoryService inventoryService)
        {
            this.inventoryService = inventoryService;
        }

        [FunctionName("StoreGet")]
        public async Task<IActionResult> StoreGet([HttpTrigger(AuthorizationLevel.Function, "get", Route = "store")] HttpRequest req, ILogger log)
        {
            try
            {
                log.LogInformation($"StoreGet for user is called");

                var items = await inventoryService.GetStoreAsync();

                return new OkObjectResult(items);
            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }

        [FunctionName("InventoryCreateItemPost")]
        public async Task<IActionResult> InventoryCreateItemPost([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "store/inventory")] HttpRequest req, ILogger log)
        {
            try
            {
                log.LogInformation("InventoryCreateItemPost is called");
                var request = await req.GetBodyAsync<CreateInventoryItemRequest>();
                await inventoryService.CreateItemAsync(request);
                return new OkObjectResult(request);
            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }

        [FunctionName("InventoryAddItemStockPut")]
        public IActionResult InventoryAddItemStockPut([HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "store/inventory/{itemId}")] HttpRequest req, ILogger log)
        {
            try
            {

                if (!req.Query.ContainsKey("itemId"))
                    throw new AppException(400, "Item Id is missing from query");

                var itemId = req.Query["itemId"];

                log.LogInformation($"Delete ShoppingCartDelete is called, basket Id: {itemId}");

                inventoryService.AddStock(itemId);

                return new OkResult();
            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }
    }
}
