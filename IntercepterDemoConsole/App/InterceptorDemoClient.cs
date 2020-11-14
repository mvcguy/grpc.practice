using Grpc.Core;
using Grpc.Net.Client;
using HelloIntercepter;
using IntercepterDemoConsole.Interceptors;
using Grpc.Core.Interceptors;
using System;
using System.Threading.Tasks;
using static HelloIntercepter.HelloIntercepterService;
using CommonLibrary;

namespace IntercepterDemoConsole.App
{
    public static class InterceptorDemoClient
    {
        static readonly GrpcChannel Channel = GrpcChannel.ForAddress("https://localhost:5001");
        private static readonly CallInvoker invoker = Channel.Intercept(new HelloClientInterceptor());
        static readonly HelloIntercepterServiceClient Client = new HelloIntercepterServiceClient(invoker);

        public static async Task Run()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            await DemoUnaryCall();
            await DemoClientStream();
            await DemoServerStream();
            await DemoDuplexStream();
            Console.ResetColor();
        }

        private static async Task DemoUnaryCall()
        {
            var reply = await Client.SendToUniverseAsync(new Message
            {
                Id = "sha-171",
                Contents = "Alhamdulillah - All praise to Allah, " +
                "THE CREATOR of everything that we see, and that we dont see"
            });
            WriteConsole(reply);
        }

        private static async Task DemoClientStream()
        {
            var call = Client.UploadToUniverse();
            await call.RequestStream.WriteAsync(new Tales { Id = "SHA171", Contents = "WE LOVE PROPHET MUHAMMAD PEACE OF ALLAH BE UPON HIM <3 <3 <3" });
            await call.RequestStream.WriteAsync(new Tales { Id = "SHA171", Contents = "We love Him because he brought the whole of humanity to the light from clear darkness." });

            await call.RequestStream.CompleteAsync();
            var reply = await call.ResponseAsync;
            WriteConsole(reply);
        }

        private static async Task DemoServerStream()
        {
            await foreach (var item in Client.GetFromUniverse(new Inquiry { }).ResponseStream.ReadAllAsync())
            {
                WriteConsole(item);
            }
        }

        private static async Task DemoDuplexStream()
        {
            var method = Client.CommunicateToUniverse();
            var responseStrm = method.ResponseStream;
            var requestStrm = method.RequestStream;
            var count = 0;

            await SendRandomMessage(requestStrm);

            await foreach (var item in responseStrm.ReadAllAsync())
            {
                WriteConsole(item);
                await SendRandomMessage(requestStrm);
                if (count++ == 10) break;
            }

            await requestStrm.CompleteAsync();

            //
            // read any remaining messages
            //
            await foreach (var item in responseStrm.ReadAllAsync())
            {
                WriteConsole(item);
            }

        }

        private static async Task SendRandomMessage(IClientStreamWriter<Message> requestStrm)
        {
            var randomMessage = new Message
            {
                Id = Guid.NewGuid().GetHashCode().ToString(),
                Contents = Utilities.RandomSentence()
            };

            await requestStrm.WriteAsync(randomMessage);
            await Task.Delay(1000);
        }

        private static void WriteConsole(Message reply)
        {
            Console.WriteLine($"Server Reply - ID: {reply.Id}, Contents: {reply.Contents}");
            Console.WriteLine();
        }

        private static void WriteConsole(SilentReply reply)
        {
            Console.WriteLine($"Server Reply - ID: {reply.Id}, Contents: {reply.Contents}");
            Console.WriteLine();
        }


    }
}
