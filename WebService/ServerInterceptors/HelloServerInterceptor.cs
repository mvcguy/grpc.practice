using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using Microsoft.Extensions.Logging;

namespace WebService.ServerInterceptors
{
    /// <summary>
    /// taken from grpc-dotnet github samples
    /// </summary>
    public class HelloServerInterceptor : Interceptor
    {
        private readonly ILogger<HelloServerInterceptor> logger;

        public HelloServerInterceptor(ILogger<HelloServerInterceptor> logger)
        {
            this.logger = logger;
        }

        public override Task<TResponse> UnaryServerHandler<TRequest, TResponse>(TRequest request, 
            ServerCallContext context, UnaryServerMethod<TRequest, TResponse> continuation)
        {
            LogCall<TRequest, TResponse>(MethodType.Unary, context);
            return base.UnaryServerHandler(request, context, continuation);
        }

        public override Task<TResponse> ClientStreamingServerHandler<TRequest, TResponse>(
           IAsyncStreamReader<TRequest> requestStream,
           ServerCallContext context,
           ClientStreamingServerMethod<TRequest, TResponse> continuation)
        {
            LogCall<TRequest, TResponse>(MethodType.ClientStreaming, context);
            return base.ClientStreamingServerHandler(requestStream, context, continuation);
        }

        public override Task ServerStreamingServerHandler<TRequest, TResponse>(
            TRequest request,
            IServerStreamWriter<TResponse> responseStream,
            ServerCallContext context,
            ServerStreamingServerMethod<TRequest, TResponse> continuation)
        {
            LogCall<TRequest, TResponse>(MethodType.ServerStreaming, context);
            return base.ServerStreamingServerHandler(request, responseStream, context, continuation);
        }

        public override Task DuplexStreamingServerHandler<TRequest, TResponse>(
            IAsyncStreamReader<TRequest> requestStream,
            IServerStreamWriter<TResponse> responseStream,
            ServerCallContext context,
            DuplexStreamingServerMethod<TRequest, TResponse> continuation)
        {
            LogCall<TRequest, TResponse>(MethodType.DuplexStreaming, context);
            return base.DuplexStreamingServerHandler(requestStream, responseStream, context, continuation);
        }

        private void LogCall<TRequest, TResponse>(MethodType methodType, ServerCallContext context)
            where TRequest : class
            where TResponse : class
        {
            logger.LogWarning($"Starting call. Type: {methodType}. Request: {typeof(TRequest)}. Response: {typeof(TResponse)}");
            WriteMetadata(context.RequestHeaders, "caller-user");
            WriteMetadata(context.RequestHeaders, "caller-machine");
            WriteMetadata(context.RequestHeaders, "caller-os");

            void WriteMetadata(Metadata headers, string key)
            {
                var headerValue = headers.SingleOrDefault(h => h.Key == key)?.Value;
                logger.LogWarning($"{key}: {headerValue ?? "(unknown)"}");
            }
        }
    }
}
