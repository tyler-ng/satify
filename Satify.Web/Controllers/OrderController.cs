﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Satify.Services.Customer;
using Satify.Services.Order;
using Satify.Web.Serialization;
using Satify.Web.ViewModels;

namespace Satify.Web.Controllers
{
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;

        public OrderController(
            ILogger<OrderController> logger,
            IOrderService orderService,
            ICustomerService customerService
            )
        {
            _logger = logger;
            _orderService = orderService;
            _customerService = customerService;
        }

        [HttpPost("/api/invoice")]
        public ActionResult GenerateNewOrder([FromBody] InvoiceModel invoice)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _logger.LogInformation("Generating invoice");
            var order = OrderMapper.SerializeInvoiceToOrder(invoice);
            order.Customer = _customerService.GetById(invoice.CustomerId);
            _orderService.GenerateOpenOrder(order);
            return Ok();
        }

        [HttpGet("/api/order")]
        public ActionResult GetOrders()
        {
            var orders = _orderService.GetOrders();
            var orderModels = OrderMapper.SerializeOrdersToViewModels(orders);
            return Ok(orderModels);
        }

        [HttpPatch("/api/order/complete/{id}")]
        public ActionResult GetOrders(int id)
        {
            _logger.LogInformation($"Marking order {id} complete...");
            _orderService.MarkFullfilled(id);
            return Ok();
        }
        
        [HttpGet("/api/order/{id}")]
        public ActionResult GetOrdersByCustomer(int id)
        {
            _logger.LogInformation($"Getting orders from customerId {id}");
            var ordersByCustomer = _orderService.GetOrdersByCustomer(id);
            return Ok(ordersByCustomer);
        }
        
    }
}