using Grpc.Core;
using GrpcAuthDemo;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebService.Services.GrpcAuthDemoSrv
{
    public class GrpcAuthDemoServImpl : GrpcAuthDemoService.GrpcAuthDemoServiceBase
    {
        [Authorize]
        public override async Task<AccountInquiryResponse> GetAccountBalance(AccountInquiry request,
            ServerCallContext context)
        {
            await Task.Delay(100);
            return new AccountInquiryResponse
            {
                AccountBalance = int.MaxValue - 1,
                AccountNumber = request.AccountNumber
            };
        }
    }
}
