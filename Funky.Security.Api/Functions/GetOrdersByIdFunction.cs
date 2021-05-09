using System.Threading.Tasks;
using Funky.Security.Api.Application.Requests;
using Funky.Security.Api.Application.Responses;
using Funky.Security.Api.Core;
using Funky.Security.Api.Functions.Bindings;
using Funky.Security.Api.Models;
using Funky.Security.Api.ResponseGenerators;
using Funky.Security.Api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using AzureAdToken = Funky.Security.Api.Models.AzureAdToken;

namespace Funky.Security.Api.Functions
{
    public class GetOrdersByIdFunction
    {
        private readonly IGetOrdersService _getOrdersService;
        private readonly IResponseGenerator<GetOrderByIdRequest, GetOrderByIdResponse> _responseGenerator;

        public GetOrdersByIdFunction(IGetOrdersService getOrdersService, IResponseGenerator<GetOrderByIdRequest, GetOrderByIdResponse> responseGenerator)
        {
            _getOrdersService = getOrdersService;
            _responseGenerator = responseGenerator;
        }


        [FunctionName(nameof(GetOrdersByIdFunction))]
        public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/orders/{orderId}")]
            HttpRequest request, string orderId,
            [AzureAdToken("sales.bff.orders.search", "access.bff, test.bff")]
            AzureAdToken authModel)
        {
            if (authModel == null)
            {
                return _responseGenerator.GetResponse(null, Result<GetOrderByIdResponse>.Failure("Unauthorized", "User does not have authorization"));
            }

            var getOrderByIdRequest = new GetOrderByIdRequest(orderId);
            var operation = await _getOrdersService.ExecuteAsync(getOrderByIdRequest);

            var response = _responseGenerator.GetResponse(getOrderByIdRequest, operation);
            return response;
        }
    }
}