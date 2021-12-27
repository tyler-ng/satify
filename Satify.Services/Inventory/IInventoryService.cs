using System;
using System.Collections.Generic;
using Satify.Data.Models;

namespace Satify.Services.Inventory
{
    public interface IInventoryService
    {
        public List<ProductInventory> GetCurrentInventory();
        public ServiceResponse<ProductInventory> UpdateUnitsAvailable(int id, int adjustment);
        public ProductInventory GetByProductId(int productId);
        public void CreateSnapshot(ProductInventory inventory);
        public List<ProductInventorySnapShot> GetSnapshotHistory();
    }
}
