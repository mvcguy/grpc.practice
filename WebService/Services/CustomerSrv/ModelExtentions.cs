using System;
using WebService.CustomersProtos;

namespace WebService.Services.CustomerSrv
{
    public static class ModelExtentions
    {
        public static CustomersResponse ToCustomerResponse(this Customer customer)
        {
            return new CustomersResponse
            {
                Address = customer.Address,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                Id = customer.Id.ToString()
            };
        }

        public static Customer ToCustomer(this CustomersResponse customer)
        {
            return new Customer
            {
                Address = customer.Address,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
                Id = Guid.Parse(customer.Id)
            };
        }

        public static Customer ToCustomer(this CreateCustomerRequest customer)
        {
            return new Customer
            {
                Address = customer.Address,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Phone = customer.Phone,
            };
        }
    }

}
