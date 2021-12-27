﻿using System;
using System.Collections.Generic;

namespace Satify.Services.Customer
{
    public interface ICustomerService
    {
        List<Data.Models.Customer> GetAllCustomers();
        ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer);
        ServiceResponse<bool> DeleteCustomer(int id);
        Data.Models.Customer GetById(int id);
        ServiceResponse<Data.Models.Customer> EditCustomer(Data.Models.Customer customer);
    }
}
