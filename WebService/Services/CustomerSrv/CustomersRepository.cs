using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GrpcService1.Services.CustomerSrv
{
    public class CustomersRepository : ICustomersRepository
    {
        private static ConcurrentBag<Customer> Customers = new ConcurrentBag<Customer>
        {
            new Customer
            {
                FirstName="Shahid",
                LastName="Ali",
                Address="Charsadda",
                Phone="83939020",
                Id = Guid.Parse("e3cf5ae6-d149-4b80-8bd8-aea03f2b0754")
            },
            new Customer
            {
                FirstName="Safia",
                LastName="Shahid",
                Address="Karachi",
                Phone="39393939",
                Id = Guid.Parse("777780ba-eeef-4084-b088-52898c78cbc0")
            }
        };

        public async Task<Guid> CreateCustomer(Customer customer)
        {
            customer.Id = Guid.NewGuid();
            Customers.Add(customer);
            await Task.Delay(500);
            return customer.Id.Value;
        }

        public async IAsyncEnumerable<Customer> GetCustomers(params object[] args)
        {
            foreach (var item in Customers)
            {
                await Task.Delay(1000);
                yield return item;
            }
        }
    }

}
