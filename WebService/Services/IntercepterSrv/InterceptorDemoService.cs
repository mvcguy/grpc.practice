using Grpc.Core;
using HelloIntercepter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebService.Services.IntercepterSrv
{
    public class InterceptorDemoService : HelloIntercepterService.HelloIntercepterServiceBase
    {
        public override async Task<SilentReply> SendToUniverse(Message request, ServerCallContext context)
        {
            await Task.Delay(100);
            var message = $"Your message is received. ID: {request.Id}, Contents: {request.Contents}";
            return new SilentReply { Contents = message, Id = Guid.NewGuid().ToString() };
        }

        public override async Task<SilentReply> UploadToUniverse(IAsyncStreamReader<Tales> requestStream, ServerCallContext context)
        {
            await Task.Delay(100);

            return new SilentReply
            {
                Id = Guid.NewGuid().ToString(),
                Contents = $"Your request is received by universe. Hashcode: {requestStream.ReadAllAsync().GetHashCode()} "
            };
        }

        public override async Task GetFromUniverse(Inquiry request, IServerStreamWriter<Message> responseStream,
            ServerCallContext context)
        {
            await Task.Delay(100);

            await responseStream.WriteAsync(new Message
            {
                Id = Guid.NewGuid().ToString(),
                Contents = $"your inquiry is arrived. {Environment.NewLine} " +
                $"\"ID: {request.Id} - {request.Contents}\""
            });            
        }

    }
}
