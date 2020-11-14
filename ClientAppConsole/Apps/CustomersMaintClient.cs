using Grpc.Net.Client;
using System;
using Grpc.Core;
using System.Threading.Tasks;
using CommonLibrary;

namespace ClientAppConsole.Apps
{
    public static class CustomersMaintClient
    {
        static readonly GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
        static readonly CustomersMaintenance.CustomersMaintenanceClient client = new CustomersMaintenance.CustomersMaintenanceClient(channel);

        public static async Task GetAllCustomers()
        {
            var customers = client.GetAllCustomers(new CustomersRequest());
            var info = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo;
            await foreach (var item in customers.ResponseStream.ReadAllAsync())
            {
                Console.WriteLine($"{Math.Abs(item.Id.GetHashCode())} {info.ToTitleCase(item.FirstName)} {item.LastName} {Environment.NewLine} " +
                    $"{item.Address} {Environment.NewLine} {item.Phone}");
                Console.WriteLine();
            }
        }

        public static async Task CreateCustomers(int count = 10)
        {
            for (int i = 0; i < count; i++)
            {
                var newCustomer = new CreateCustomerRequest
                {
                    FirstName = Utilities.RandomString(10),
                    LastName = Utilities.RandomString(5),
                    Address = Utilities.RandomString(10) + Environment.NewLine + Utilities.RandomString(16),
                    Phone = Utilities.RandomDigits()
                };
                var result = await client.CreateCustomerAsync(newCustomer);
                await Task.Delay(100);
                Console.WriteLine("Customer with ID {0} is created successfully", result.Id);
            }
        }

        public static async Task DeleteCustomer(string id)
        {
            var result = await client.DeleteCustomerAsync(new DeleteCustomerRequest { Id = id });

            Console.WriteLine($"Delete Customer Id:{id}, Result: {result.Result} ");
        }

        public static async Task UpdateCustomer(UpdateCustomerRequest request)
        {
            var result = await client.UpdateCustomerAsync(request);
            Console.WriteLine($"Customer update result: {result.Result}, Message: {result.Message}");
        }

        public static async Task DeleteAllCustomers()
        {
            var result = await client.DeleteAllCustomersAsync(new DeleteAllCustomersRequest());
            Console.WriteLine($"Delete All customers, result: {result.Result}, Message: {result.Message}");
        }
        
    }
}
