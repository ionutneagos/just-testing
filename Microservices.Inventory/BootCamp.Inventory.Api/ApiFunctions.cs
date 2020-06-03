namespace BootCamp.Inventory.Api
{
    using BootCamp.Inventory.Services.Models.Data;
    using BootCamp.Inventory.Services.Models.Request;
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

        [FunctionName("StoreGet")]
        public static async Task<IActionResult> StoreGet([HttpTrigger(AuthorizationLevel.Function, "get", Route = "store")] HttpRequest req, [DurableClient] IDurableClient client, ILogger log)
        {
            try
            {
                log.LogInformation($"Store Get is called");

                var catalogEntity = new EntityId(nameof(Inventory), Inventory.Id);

                var response = await client.ReadEntityStateAsync<Inventory>(catalogEntity);

                if (!response.EntityExists)
                    return new NotFoundResult();

                return new OkObjectResult(response.EntityState.Items);
            }

            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }

        [FunctionName("InventoryCreateItemPost")]
        public static async Task<IActionResult> InventoryCreateItemPost([HttpTrigger(AuthorizationLevel.Function, "POST", Route = "store/inventory")] HttpRequest req,
            [DurableClient] IDurableClient client, ILogger log)
        {
            try
            {
                log.LogInformation("InventoryCreateItemPost is called");
                var request = await req.GetBodyAsync<CreateInventoryItemRequest>();

                var inventoryItem = new InventoryItem(description: request.Description, price: request.Price,
                                                    availableStock: request.AvailableStock, restockThreshold: request.RestockThreshold, maxStockThreshold: request.MaxStockThreshold);

                await client.SignalEntityAsync<IInventory>(Inventory.Id, x => x.CreateItemAsync(inventoryItem));

                return new OkObjectResult(request);
            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }

        [FunctionName("InventoryAddItemStockPut")]
        public static async Task<IActionResult> InventoryAddItemStockPutAsync([HttpTrigger(AuthorizationLevel.Function, "PUT", Route = "store/inventory/{itemId}")] HttpRequest req,
            [DurableClient] IDurableClient client, ILogger log)
        {
            try
            {

                if (!req.Query.ContainsKey("itemId"))
                    throw new AppException(400, "Item Id is missing from query");

                var itemId = req.Query["itemId"];
                var quantity =Convert.ToInt32(req.Query["quantity"]);

                log.LogInformation($"Delete ShoppingCartDelete is called, basket Id: {itemId}");

                await client.SignalEntityAsync<IInventory>(Inventory.Id, x => x.AddStock(itemId, quantity));

                return new OkResult();
            }
            catch (Exception ex)
            {
                return ex.GetResponse(log);
            }
        }
    }
}
