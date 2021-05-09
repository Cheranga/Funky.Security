using System.Net;
using Funky.Security.Api.Application.Requests;
using Funky.Security.Api.Application.Responses;
using Funky.Security.Api.Core;
using Microsoft.AspNetCore.Mvc;

namespace Funky.Security.Api.ResponseGenerators
{
    public class GetOrdersByIdFunctionResponseGenerator : ResponseGeneratorBase<GetOrderByIdRequest, GetOrderByIdResponse>
    {
        protected override IActionResult GetSpecificResponse(GetOrderByIdRequest request, Result<GetOrderByIdResponse> result)
        {
            if (result.Status)
            {
                return GetSuccessResponse(result);
            }

            var errorCode = result.ErrorCode;

            var response = errorCode switch
            {
                "InvalidRequest" => GetErrorResponse("InvalidRequest", HttpStatusCode.BadRequest),
                _ => GetErrorResponse("Internal server error occured", HttpStatusCode.InternalServerError)
            };

            return response;
        }
    }
}