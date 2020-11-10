using Grpc.Core;
using Grpc.Net.Client;
using HelloIntercepter;
using IntercepterDemoConsole.Interceptors;
using Grpc.Core.Interceptors;
using System;
using System.Threading.Tasks;
using static HelloIntercepter.HelloIntercepterService;

namespace IntercepterDemoConsole.App
{
    public static class InterceptorDemoClient
    {
        static readonly GrpcChannel Channel = GrpcChannel.ForAddress("https://localhost:5001");
        private static readonly CallInvoker invoker = Channel.Intercept(new HelloClientInterceptor());
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
