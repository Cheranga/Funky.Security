using System;
using Microsoft.Azure.WebJobs.Description;

namespace Funky.Security.Api.Functions.Bindings
{
    [Binding]
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
    public class AzureAdTokenAttribute : Attribute
    {
        public AzureAdTokenAttribute() : this(string.Empty, string.Empty,string.Empty, string.Empty, string.Empty)
        {
            
        }
        public AzureAdTokenAttribute(string roles, string scopes): this(string.Empty, string.Empty, string.Empty, roles, scopes)
        {
        }
        
        public AzureAdTokenAttribute(string tenantId, string clientId, string audience, string roles, string scopes)
        {
            TenantId = tenantId;
            ClientId = clientId;
            Audience = audience;
            Roles = roles;
            Scopes = scopes;
        }
        
        [AutoResolve] public string Scopes { get;}
        [AutoResolve] public string Roles { get; }
        [AutoResolve] public string Audience { get; }
        [AutoResolve] public string TenantId { get; }
        [AutoResolve] public string ClientId { get; }

        public string AuthorizeUrl => $"https://login.microsoft.com/{TenantId}/v2.0/.well-known/openid-configuration";
    }
}