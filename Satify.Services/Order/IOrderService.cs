using System;
using System.Collections.Generic;
using Satify.Data.Models;

namespace Satify.Services.Order
{
    public interface IOrderService
    {
        List<SalesOrder> GetOrders();
        ServiceResponse<bool> GenerateOpenOrder(SalesOrder order);
        ServiceResponse<bool> MarkFullfilled(int id);
        List<SalesOrder> GetOrdersByCustomer(int customerId);
    }
}
