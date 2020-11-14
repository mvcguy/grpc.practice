using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using CommonLibrary;
using GrpcService1.Services.CustomerSrv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using WebService.Properties;
using WebService.ServerInterceptors;
using WebService.Services.IntercepterSrv;
using WebService.Services.PlayDiceSrv;

namespace GrpcService1
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc(options => { options.Interceptors.Add<HelloServerInterceptor>(); });
            services.AddSingleton<ICustomersRepository, CustomersRepository>();

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
                        IssuerSigningKey = FakeKeyStore.key,
                        RequireAudience = false,
                        ValidateAudience = false,
                        ValidateIssuer = false,
                        ValidateActor = false,
                        ValidateLifetime = true
                        
                    };
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>();
                endpoints.MapGrpcService<CustomersMaintenanceService>();
                endpoints.MapGrpcService<PlayDiceService>();
                endpoints.MapGrpcService<InterceptorDemoService>();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");
                });
            });
        }
    }

}
