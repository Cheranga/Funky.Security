using System;
using System.Threading.Tasks;
using Funky.Security.Api.Functions.Bindings;
using Funky.Security.Api.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace Funky.Security.Api.Functions
{
    public class SearchOrderByIdFunction
    {
        [FunctionName(nameof(SearchOrderByIdFunction))]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "search/orders/{orderId}")]
            HttpRequest request,
            string orderId,
            [AzureAdToken("%AzureAd:TenantId%", "%AzureAd:ClientId%", "%AzureAd:Audience%", "%AzureAd:Roles%", "%AzureAd:Scopes%")]
            AzureAdToken token)
        {
            if (token == null)
            {
                return new UnauthorizedResult();
            }

            await Task.Delay(TimeSpan.FromSeconds(2));

            return new OkObjectResult($"Order search results for {orderId}");
        }
    }
}