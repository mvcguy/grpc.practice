using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcService1.Services.CustomerSrv
{
    public interface ICustomersRepository
    {
        IAsyncEnumerable<Customer> GetCustomers(params object[] args);

        Task<Guid> CreateCustomer(Customer customer);
        Task<Customer> GetCustomer(Guid id);
        Task<bool> DeleteCustomer(Guid id);
    }

}
