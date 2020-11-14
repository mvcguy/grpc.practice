using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.DotNet.PlatformAbstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace IntercepterDemoConsole.Interceptors
{
    public class HelloClientInterceptor : Interceptor
    {
        public override AsyncUnaryCall<TResponse> AsyncUnaryCall<TRequest, TResponse>(TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncUnaryCallContinuation<TRequest, TResponse> continuation)
        {
            context = AddMetadata(context);
            LogCall(context.Method);
            return base.AsyncUnaryCall(request, context, continuation);
        }

        public override AsyncDuplexStreamingCall<TRequest, TResponse>
            AsyncDuplexStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context,
            AsyncDuplexStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            context = AddMetadata(context);
            LogCall(context.Method);
            return base.AsyncDuplexStreamingCall(context, continuation);
        }

        public override AsyncClientStreamingCall<TRequest, TResponse>
            AsyncClientStreamingCall<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context,
            AsyncClientStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            context = AddMetadata(context);
            LogCall(context.Method);
            return base.AsyncClientStreamingCall(context, continuation);
        }

        public override AsyncServerStreamingCall<TResponse> AsyncServerStreamingCall<TRequest, TResponse>(TRequest request,
            ClientInterceptorContext<TRequest, TResponse> context,
            AsyncServerStreamingCallContinuation<TRequest, TResponse> continuation)
        {
            context = AddMetadata(context);
            LogCall(context.Method);
            return base.AsyncServerStreamingCall(request, context, continuation);
        }

        private void LogCall<TRequest, TResponse>(Method<TRequest, TResponse> method)
           where TRequest : class
           where TResponse : class
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"CLIENT INTERCEPTOR - Handling a GRPC call. Request: {typeof(TRequest)}, " +
                $"Response: {typeof(TResponse)}, Method: {method.Name}");
            Console.WriteLine();
            Console.ResetColor();
        }

        private static ClientInterceptorContext<TRequest, TResponse> AddMetadata<TRequest, TResponse>(ClientInterceptorContext<TRequest, TResponse> context)
            where TRequest : class
            where TResponse : class
        {
            var headers = context.Options.Headers;
            if (headers == null)
            {
                headers = new Metadata()
                {
                    new Metadata.Entry("caller-user", Environment.UserName),
                    new Metadata.Entry("caller-machine", Environment.MachineName),
                    new Metadata.Entry("caller-os",
                    $"OS: {RuntimeEnvironment.OperatingSystem}, " +
                    $"Platform: {RuntimeEnvironment.OperatingSystemPlatform}, " +
                    $"Version: {RuntimeEnvironment.OperatingSystemVersion}"),
                };
                var options = context.Options.WithHeaders(headers);
                context = new ClientInterceptorContext<TRequest, TResponse>(context.Method, context.Host, options);
            }

            return context;
        }



    }
}
