using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Globalization;
using ClientAppConsole.Apps;

namespace ClientAppConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HelloWorldClient.Hello("Shahid");

            await CustomersMaintClient.CreateCustomers(2);

            var update = new UpdateCustomerRequest
            {
                Id = "e3cf5ae6-d149-4b80-8bd8-aea03f2b0754",
                FirstName = "Shahid Ali",
                LastName = "Khan",
                Address = "Charsadda, Pakistan",
                Phone = "123490000"
            };
            await CustomersMaintClient.UpdateCustomer(update);
            await CustomersMaintClient.GetAllCustomers();
            await CustomersMaintClient.DeleteCustomer("e3cf5ae6-d149-4b80-8bd8-aea03f2b0754");
            await CustomersMaintClient.GetAllCustomers();

            await CustomersMaintClient.DeleteAllCustomers();
            await CustomersMaintClient.GetAllCustomers();
            await CustomersMaintClient.CreateCustomers(10);
            await CustomersMaintClient.GetAllCustomers();

            Console.ReadLine();
        }
    }
}
