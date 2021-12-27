using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Satify.Data;

namespace Satify.Services.Customer
{
    public class CustomerService : ICustomerService
    {

        private readonly TofuDbContext _db;

        public CustomerService(TofuDbContext dbContext)
        {
            _db = dbContext;
        }

        /// <summary>
        /// Adds a new Customer record
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <returns>ServiceResponse<Customer></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer)
        {
            try
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = true,
                    Message = "New customer added",
                    Time = DateTime.UtcNow,
                    Data = customer
                };
            } catch(Exception e)
            {
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = false,
                    Message = "creating a customer failed",
                    Time = DateTime.UtcNow,
                    Data = customer
                };
            }
        }

        /// <summary>
        /// Delete a customer record
        /// </summary>
        /// <param name="id"></param>
        /// <returns>ServiceResponse<bool></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ServiceResponse<bool> DeleteCustomer(int id)
        {
            var customer = _db.Customers.Find(id);
            var now = DateTime.UtcNow;

            if (customer == null)
            {
                return new ServiceResponse<bool>
                {
                    Time = now,
                    IsSuccess = false,
                    Message = "Customer to delete not found!",
                    Data = false,
                };
            }

            try
            {
                _db.Customers.Remove(customer);
                _db.SaveChanges();

                return new ServiceResponse<bool>
                {
                    Time = now,
                    IsSuccess = true,
                    Message = "Customer created!",
                    Data = false,
                };
            } catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    Time = now,
                    IsSuccess = false,
                    Message = e.StackTrace,
                    Data = false,
                };
            }
            
        }

        /// <summary>
        /// Returns a list of Customers from the database
        /// </summary>
        /// <returns></returns>
        public List<Data.Models.Customer> GetAllCustomers()
        {
            return _db.Customers
                .Include(customer => customer.PrimaryAddress)
                .OrderBy(customer => customer.LastName)
                .ToList();
        }

        /// <summary>
        /// Gets a customer record by primary key
        /// </summary>
        /// <param name="id">int customer primary key</param>
        /// <returns>Customer</returns>
        public Data.Models.Customer GetById(int id)
        {
            // return _db.Customers.Find(id);
            return _db.Customers
                .Include(customer => customer.PrimaryAddress)
                .First(customer => customer.Id == id);
        }

        /// <summary>
        /// Edit details of a customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ServiceResponse<Data.Models.Customer> EditCustomer(Data.Models.Customer customer)
        {
            var newCustomer = _db.Customers.Find(customer.Id);
            newCustomer.FirstName = customer.FirstName;
            newCustomer.LastName = customer.LastName;
            newCustomer.PrimaryAddress = customer.PrimaryAddress;
            newCustomer.UpdatedOn = customer.UpdatedOn;
            var now = DateTime.UtcNow;
            
            try
            {
                _db.SaveChanges();

                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = true,
                    Message = "Updated Customer",
                    Data = customer,
                    Time = now
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = false,
                    Message = "Update Customer failed",
                    Data = customer,
                    Time = now
                };
            }
        }
    }
}
