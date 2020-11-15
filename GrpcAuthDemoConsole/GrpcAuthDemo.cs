using Grpc.Core;
using Grpc.Net.Client;
using GrpcAuthDemo;
using System;
using System.Threading.Tasks;

namespace GrpcAuthDemoConsole
{
    public static class GrpcAuthDemo
    {
        static GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
        static GrpcAuthDemoService.GrpcAuthDemoServiceClient client = new GrpcAuthDemoService.GrpcAuthDemoServiceClient(channel);
        public static async Task Run(string token)
        {
            var headers = new Metadata();
            headers.Add("Authorization", $"Bearer {token}");

            var result = await client.GetAccountBalanceAsync(new AccountInquiry { AccountNumber = "1129", Hash = "xxx" }, headers);
            Console.WriteLine($"Server-reply: Account: {result.AccountNumber}, Balance: {result.AccountBalance}.{result.AccountBalanceDecimal}");

        }
    }


}
