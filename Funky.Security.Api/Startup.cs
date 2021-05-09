using System.IdentityModel.Tokens.Jwt;
using Funky.Security.Api;
using Funky.Security.Api.Application.Requests;
using Funky.Security.Api.Application.Responses;
using Funky.Security.Api.Functions.Bindings;
using Funky.Security.Api.ResponseGenerators;
using Funky.Security.Api.Services;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host.Bindings;
using Microsoft.Azure.WebJobs.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

[assembly: WebJobsStartup(typeof(Startup))]

namespace Funky.Security.Api
{
    public class Startup : IWebJobsStartup
    {
        public void Configure(IWebJobsBuilder builder)
        {
            builder.UseAzureAdTokenBinding();
            var services = builder.Services;

            RegisterServices(services);
            RegisterResponseFormatters(services);
        }

        private void RegisterResponseFormatters(IServiceCollection services)
        {
            services.AddScoped<IResponseGenerator<GetOrderByIdRequest, GetOrderByIdResponse>, GetOrdersByIdFunctionResponseGenerator>();
        }

        protected virtual IConfigurationRoot GetConfigurationRoot(IWebJobsBuilder builder)
        {
            var services = builder.Services;

            var executionContextOptions = services.BuildServiceProvider().GetService<IOptions<ExecutionContextOptions>>().Value;

            var configuration = new ConfigurationBuilder()
                .SetBasePath(executionContextOptions.AppDirectory)
                .AddJsonFile("local.settings.json", true, true)
                .AddEnvironmentVariables()
                .Build();

            return configuration;
        }

        private void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IGetOrdersService, GetOrdersService>();
        }
    }
}