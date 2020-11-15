using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using CommonLibrary;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using WebService.Controllers;
using WebService.ServerInterceptors;
using WebService.Services;
using WebService.Services.CustomerSrv;
using WebService.Services.GrpcAuthDemoSrv;
using WebService.Services.IntercepterSrv;
using WebService.Services.PlayDiceSrv;

namespace WebService
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(options => { options.Interceptors.Add<HelloServerInterceptor>(); });
            services.AddGrpcReflection();
            services.AddSingleton<ICustomersRepository, CustomersRepository>();
            services.AddSingleton<JwtSecurityTokenHandler>();
            services.AddSingleton<ITokenSource, FakeTokenSource>();

            services
                .AddAuthorization(configure =>
                {
                    configure.AddPolicy(JwtBearerDefaults.AuthenticationScheme, configPolicy =>
                    {
                        configPolicy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                        configPolicy.RequireClaim(ClaimTypes.Email);
                    });
                })
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = FakeKeyStore.Key,
                        RequireAudience = false,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateActor = false,
                        ValidateLifetime = true

                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ITokenSource tokenSource, ILogger<FakeTokenController> logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints =>
                {
                    endpoints.MapGrpcService<GreeterService>();
                    endpoints.MapGrpcService<CustomersMaintenanceService>();
                    endpoints.MapGrpcService<PlayDiceService>();
                    endpoints.MapGrpcService<InterceptorDemoService>();
                    endpoints.MapGrpcService<GrpcAuthDemoServImpl>();
                    endpoints.MapGrpcReflectionService();

                    endpoints.MapGet("/jwt/token", async context =>
                    {
                        var controller = new FakeTokenController(tokenSource, context, logger);
                        var result = await controller.GetToken();
                        context.Response.StatusCode = result.StatusCode;
                        await context.Response.WriteAsync(result.Result);
                    });

                    endpoints.MapGet("/", async context =>
                    {
                        await context.Response.WriteAsync("Grpc client is ready for requests");
                    });
                });
        }
    }

}
