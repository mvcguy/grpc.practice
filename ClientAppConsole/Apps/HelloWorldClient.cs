using Grpc.Net.Client;
using System;

namespace ClientAppConsole.Apps
{
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
}
