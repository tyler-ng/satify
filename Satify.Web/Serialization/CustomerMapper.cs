using System;
using Satify.Data.Models;
using Satify.Web.ViewModels;

namespace Satify.Web.Serialization
{
    public class CustomerMapper
    {
        /// <summary>
        /// Serializes a Customer data model into a CustomerModel view model
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static CustomerModel SerializeCustomer(Customer customer)
        {
            return new CustomerModel
            {
                Id = customer.Id,
                CreatedOn = customer.CreatedOn,
                UpdatedOn = customer.UpdatedOn,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PrimaryAddress = MapCustomerAddress(customer.PrimaryAddress)
            };
        }

        /// <summary>
        /// Serializes a CustomerModel view model into a Customer data model
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static Customer SerializeCustomer(CustomerModel customer)
        {
            return new Customer
            {
                Id = customer.Id,
                CreatedOn = customer.CreatedOn,
                UpdatedOn = customer.UpdatedOn,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PrimaryAddress = MapCustomerAddress(customer.PrimaryAddress)
            };
        }


        /// <summary>
        /// Maps a CustomerAddress data model to a CustomerAddressModel view model
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static CustomerAddressModel MapCustomerAddress(CustomerAddress address)
        {
            return new CustomerAddressModel
            {
                Id = address.Id,
                CreatedOn = address.CreatedOn,
                UpdatedOn = address.UpdatedOn,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                Province = address.Province,
                PostalCode = address.PostalCode,
                Country = address.Country
            };
        }

        /// <summary>
        /// Maps a CustomerAddressModel view model to a CustomerAddress data model
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static CustomerAddress MapCustomerAddress(CustomerAddressModel address)
        {
            return new CustomerAddress
            {
                Id = address.Id,
                CreatedOn = address.CreatedOn,
                UpdatedOn = address.UpdatedOn,
                AddressLine1 = address.AddressLine1,
                AddressLine2 = address.AddressLine2,
                City = address.City,
                Province = address.Province,
                PostalCode = address.PostalCode,
                Country = address.Country
            };
        }
    }
}
