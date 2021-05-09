namespace Funky.Security.Api.Application.Requests
{
    public class GetOrderByIdRequest
    {
        public GetOrderByIdRequest(string orderId)
        {
            OrderId = orderId;
        }

        public string OrderId { get; }
    }
}