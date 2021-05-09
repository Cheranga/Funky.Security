using System.Collections.Generic;
using Funky.Security.Api.Application.Assets;

namespace Funky.Security.Api.Application.Responses
{
    public class GetOrderByIdResponse
    {
        public string OrderId { get; set; }
        public List<Product> Products { get; set; } = new List<Product>();
    }
}