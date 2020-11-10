using Grpc.Core;
using Grpc.Core.Interceptors;
using Grpc.Net.Client;
using HelloIntercepter;
using System;
using System.Threading.Tasks;
using static HelloIntercepter.HelloIntercepterService;

namespace IntercepterDemoConsole
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            await InterceptorDemoClient.Run();

            Console.WriteLine("Press any key to exist");

            Console.ReadKey();
        }
    }

    public static class InterceptorDemoClient
    {
        static readonly GrpcChannel Channel = GrpcChannel.ForAddress("https://localhost:5001");
        static readonly CallInvoker invoker = Channel.Intercept(new HelloClientInterceptor());
        static readonly HelloIntercepterServiceClient Client = new HelloIntercepterServiceClient(invoker);

        public static async Task Run()
        {
            var reply = await Client.SendToUniverseAsync(new Message
            {
                Id = "sha-171",
                Contents = "Alhamdulillah - All praise to Allah, " +
                "THE CREATOR of everything that we see, and that we dont see"
            });
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Server Reply - ID: {reply.Id}, Contents: {reply.Contents}");
            Console.ResetColor();
        }
    }
}
