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


    }
}
