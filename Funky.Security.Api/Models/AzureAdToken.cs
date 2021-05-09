using System.Security.Claims;

namespace Funky.Security.Api.Models
{
    public class AzureAdToken
    {
        public ClaimsPrincipal User { get; set; }

    }
}