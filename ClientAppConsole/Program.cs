using Grpc.Net.Client;
using System;
using System.Collections.Generic;
using Grpc.Core;
using System.Threading.Tasks;
using System.Globalization;

namespace ClientAppConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HelloWorldClient.Hello("Shahid");

            await CustomersMaintClient.CreateCustomer(2);
            CustomersMaintClient.GetAllCustomers();
            await Task.Delay(100);
            await CustomersMaintClient.DeleteCustomer("e3cf5ae6-d149-4b80-8bd8-aea03f2b0754");
            //await CustomersMaintClient.GetAllCustomers();
            Console.ReadLine();
        }
    }

    public static class HelloWorldClient
    {
        public static void Hello(string name)
        {
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);

            var reply = client.SayHello(new HelloRequest { Name = name });
            Console.WriteLine(reply.Message);
        }
    }

    public static class CustomersMaintClient
    {
        static GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
        static CustomersMaintenance.CustomersMaintenanceClient client = new CustomersMaintenance.CustomersMaintenanceClient(channel);

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

        public static async Task CreateCustomer(int count = 10)
        {
            for (int i = 0; i < count; i++)
            {
                var newCustomer = new CreateCustomerRequest
                {
                    FirstName = RandomString(10),
                    LastName = RandomString(5),
                    Address = RandomString(10) + Environment.NewLine + RandomString(16),
                    Phone = RandomDigits()
                };
                var result = await client.CreateCustomerAsync(newCustomer);
                await Task.Delay(100);
                Console.WriteLine("Customer with ID {0} is created successfully", result.Id);
            }
        }

        public static async Task DeleteCustomer(string id)
        {
            var result = await client.DeleteCustomerAsync(new DeleteCustomerRequest { Id = id });

            Console.WriteLine($"Customer delete Id:{id}, Result: {result.Result} ");
        }

        public static string RandomString(int length = 5)
        {
            var str = string.Empty;
            for (int i = 0; i < length; i++)
            {
                str += (char)(new Random(Guid.NewGuid().GetHashCode()).Next(97, 122));
            }
            return str;
        }
        public static string RandomDigits(int length = 8)
        {
            var str = string.Empty;
            for (int i = 0; i < length; i++)
            {
                str += (char)(new Random(Guid.NewGuid().GetHashCode()).Next(48, 57));
            }
            return str;
        }
    }
}
