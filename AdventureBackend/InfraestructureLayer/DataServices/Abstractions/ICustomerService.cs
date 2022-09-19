using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DomainLayer.DomainModels;

namespace DomainLayer.Abstractions;

public interface ICustomerService
{
    Task<CustomerResponse> GetCustomers();
    
    Task<CustomerResponse> GetCustomer(int id);

    Task<CustomerResponse> UpdateCustomer(Customer customerRequest, int id);

    Task<CustomerResponse> DeleteCustomer(int id);

    Task<CustomerResponse> CreateCustomer(Customer customerRequest);

}
