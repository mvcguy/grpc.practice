using System;
using System.Linq;
using System.Threading.Tasks;
using CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebService.Models;

namespace WebService.Controllers
{
    public class FakeTokenController
    {
        private readonly ITokenSource tokenSource;
        private readonly HttpContext context;
        private readonly ILogger<FakeTokenController> logger;

        public FakeTokenController(ITokenSource tokenSource, HttpContext context, ILogger<FakeTokenController> logger)
        {
            this.tokenSource = tokenSource;
            this.context = context;
            this.logger = logger;
        }

        public async Task<EndpointResult> GetToken()
        {
            try
            {
                if (!context.Request.Headers.TryGetValue("email", out var values)
                           || !values.Any() || string.IsNullOrWhiteSpace(values[0]))
                {
                    return new EndpointResult
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Result = "email is required"
                    };
                }

                var email = values[0];
                return new EndpointResult
                {
                    Result = await tokenSource.GetTokenAsync(email),
                    StatusCode = StatusCodes.Status200OK
                };
            }
            catch (Exception e)
            {
                logger.LogError(e, e.Message);
                return new EndpointResult
                {
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Result = "An error has occurred. Try again later"
                };
            }

        }
    }

}
