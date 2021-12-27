using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Satify.Data.Models;
using Satify.Services.Customer;
using Satify.Web.Serialization;
using Satify.Web.ViewModels;

namespace Satify.Web.Controllers
{
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _logger = logger;
            
            _customerService = customerService;
        }

        [HttpPost("/api/customer")]
        public ActionResult CreateCustomer([FromBody] CustomerModel customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var now = DateTime.UtcNow;
            _logger.LogInformation("Creating a new customer");
            customer.CreatedOn = now;
            customer.UpdatedOn = now;
            var customerData = CustomerMapper.SerializeCustomer(customer);
            var newCustomer = _customerService.CreateCustomer(customerData);
            return Ok(newCustomer);
        }

        [HttpGet("/api/customer")]
        public ActionResult GetCustomers()
        {
            _logger.LogInformation("Getting customers");
            var customers = _customerService.GetAllCustomers();
            var customerModels = customers
                .Select(customer => CustomerMapper.SerializeCustomer(customer))
                .OrderByDescending(customer => customer.CreatedOn)
                .ToList();

            return Ok(customerModels);
        }

        [HttpGet("/api/customer/{id}")]
        public ActionResult GetCustomerById(int id)
        {
            _logger.LogInformation("Getting a customer");
            var customer = _customerService.GetById(id);
            var customerModel = CustomerMapper.SerializeCustomer(customer);
            return Ok(customerModel);
        }

        [HttpDelete("/api/customer/{id}")]
        public ActionResult DeleteCustomer(int id)
        {
            _logger.LogInformation("Deleting a customer");
            var response = _customerService.DeleteCustomer(id);
            return Ok(response);
        }

        [HttpPatch("/api/customer/")]
        public ActionResult EditCustomer([FromBody] CustomerModel customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            _logger.LogInformation("Updating an existing customer");
            customer.UpdatedOn = DateTime.UtcNow;
            var newCustomer = CustomerMapper.SerializeCustomer(customer);
            var updatedCustomer = _customerService.EditCustomer(newCustomer);
            return Ok(updatedCustomer);
        }
    }
}
