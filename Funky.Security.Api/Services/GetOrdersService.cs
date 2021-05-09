using System;
using System.Linq;
using System.Threading.Tasks;
using Funky.Security.Api.Application.Assets;
using Funky.Security.Api.Application.Requests;
using Funky.Security.Api.Application.Responses;
using Funky.Security.Api.Core;

namespace Funky.Security.Api.Services
{
    public class GetOrdersService : IGetOrdersService
    {
        public Task<Result<GetOrderByIdResponse>> ExecuteAsync(GetOrderByIdRequest request)
        {
            var response = new GetOrderByIdResponse
            {
                OrderId = request.OrderId,
                Products = Enumerable.Range(1, request.OrderId.Length).Select(x => new Product
                {
                    ProductId = $"PROD_{x}",
                    Name = $"Product name {x}",
                    Description = $"Product description {x}"
                }).ToList()
            };
            
            return Task.FromResult(Result<GetOrderByIdResponse>.Success(response));
        }
    }
}