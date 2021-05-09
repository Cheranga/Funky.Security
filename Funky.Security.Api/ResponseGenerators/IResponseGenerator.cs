using Funky.Security.Api.Core;
using Microsoft.AspNetCore.Mvc;

namespace Funky.Security.Api.ResponseGenerators
{
    public interface IResponseGenerator<TRequest, TResponse> where TRequest : class where TResponse : class
    {
        IActionResult GetResponse(TRequest request, Result<TResponse> result);
    }
}