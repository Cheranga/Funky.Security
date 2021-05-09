using System;
using System.Net;
using System.Web.Http;
using Funky.Security.Api.Core;
using Microsoft.AspNetCore.Mvc;

namespace Funky.Security.Api.ResponseGenerators
{
    public abstract class ResponseGeneratorBase<TRequest, TResponse> : IResponseGenerator<TRequest, TResponse> where TRequest : class where TResponse : class
    {
        public virtual IActionResult GetResponse(TRequest request, Result<TResponse> result)
        {
            if (result == null)
            {
                return new InternalServerErrorResult();
            }

            if (string.Equals("Unauthorized", result.ErrorCode, StringComparison.OrdinalIgnoreCase))
            {
                return new ObjectResult("User is not authroized to carry out this operation")
                {
                    StatusCode = (int) (HttpStatusCode.Unauthorized)
                };
            }

            return GetSpecificResponse(request, result);
        }

        protected virtual IActionResult GetErrorResponse(string error, HttpStatusCode statusCode)
        {
            return new ObjectResult(error)
            {
                StatusCode = (int) (statusCode)
            };
        }

        protected virtual IActionResult GetSuccessResponse(Result<TResponse> result)
        {
            return new OkObjectResult(result?.Data);
        }

        protected abstract IActionResult GetSpecificResponse(TRequest request, Result<TResponse> result);
    }
}