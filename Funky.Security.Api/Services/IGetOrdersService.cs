using System.Threading.Tasks;
using Funky.Security.Api.Application.Requests;
using Funky.Security.Api.Application.Responses;
using Funky.Security.Api.Core;

namespace Funky.Security.Api.Services
{
    public interface IGetOrdersService
    {
        Task<Result<GetOrderByIdResponse>> ExecuteAsync(GetOrderByIdRequest request);
    }
}