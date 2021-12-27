using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Satify.Services.Inventory;
using Satify.Web.Serialization;
using Satify.Web.ViewModels;

namespace Satify.Web.Controllers
{
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly ILogger<InventoryController> _logger;
        private readonly IInventoryService _inventoryService;

        public InventoryController(ILogger<InventoryController> logger, IInventoryService inventoryService)
        {
            _logger = logger;
            _inventoryService = inventoryService;
        }

        [HttpGet("/api/inventory")]
        public ActionResult GetCurrentInventory ()
        {
            _logger.LogInformation("Getting all inventory...");
            var inventory = _inventoryService.GetCurrentInventory()
                .Select(pi => new ProductInventoryModel {
                    Id = pi.Id,
                    CreatedOn = pi.CreatedOn,
                    UpdatedOn = pi.UpdatedOn,
                    Product = ProductMapper.SerializeProductModel(pi.Product),
                    QuantityOnHand = pi.QuantityOnHand,
                    IdealQuantity = pi.IdealQuantity
                })
                .OrderBy(inv => inv.Product.Name)
                .ToList();

            return Ok(inventory);
        }

        [HttpPatch("/api/inventory")]
        public ActionResult UpdateInventory([FromBody] ShipmentModel shipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _logger.LogInformation(
                "Updating inventory" +
                $"for {shipment.ProductId} - " +
                $"Adjustment: {shipment.Adjustment}");

            var id = shipment.ProductId;
            var adjustment = shipment.Adjustment;
            var inventory = _inventoryService.UpdateUnitsAvailable(id, adjustment);
            return Ok(inventory);
        }
    }
}
