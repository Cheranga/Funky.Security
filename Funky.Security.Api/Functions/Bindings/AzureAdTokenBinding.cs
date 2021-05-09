using System.Threading;
using System.Threading.Tasks;
using Funky.Security.Api.Models;
using Microsoft.Azure.WebJobs.Description;
using Microsoft.Azure.WebJobs.Host.Config;
using Microsoft.Extensions.Configuration;

namespace Funky.Security.Api.Functions.Bindings
{
    [Extension(nameof(AzureAdTokenBinding))]
    public class AzureAdTokenBinding : IExtensionConfigProvider
    {
        private const string AzureAd = nameof(AzureAd);
        private readonly AzureAdTokenValidationService _customAuthorizationService;
        private readonly IConfiguration _configuration;

        public AzureAdTokenBinding(AzureAdTokenValidationService customAuthorizationService, IConfiguration configuration)
        {
            _customAuthorizationService = customAuthorizationService;
            _configuration = configuration;
        }

        public void Initialize(ExtensionConfigContext context)
        {
            var rule = context.AddBindingRule<AzureAdTokenAttribute>();
            rule.BindToInput(GetAzureAdTokenAsync);
        }

        private async Task<AzureAdToken> GetAzureAdTokenAsync(AzureAdTokenAttribute attribute, CancellationToken cancellationToken)
        {
            if (attribute == null)
            {
                return null;
            }

            var tokenAttribute = GetAttributeWithRequiredSettings(attribute);

            var claimsPrincipal = await _customAuthorizationService.GetClaimsPrincipalAsync(tokenAttribute);
            if (claimsPrincipal == null)
            {
                return null;
            }

            return new AzureAdToken
            {
                User = claimsPrincipal
            };
        }

        private AzureAdTokenAttribute GetAttributeWithRequiredSettings(AzureAdTokenAttribute attribute)
        {
            var configuredAttributeData = _configuration.GetSection(AzureAd).Get<TokenConfig>();

            var audience = string.IsNullOrWhiteSpace(attribute?.Audience) ? configuredAttributeData.Audience : attribute.Audience;
            var tenantId = string.IsNullOrWhiteSpace(attribute?.TenantId) ? configuredAttributeData.TenantId : attribute.TenantId;
            var clientId = string.IsNullOrWhiteSpace(attribute?.ClientId) ? configuredAttributeData.ClientId : attribute.ClientId;
            var roles = string.IsNullOrWhiteSpace(attribute?.Roles) ? configuredAttributeData.Roles : attribute.Roles;
            var scopes = string.IsNullOrWhiteSpace(attribute?.Scopes) ? configuredAttributeData.Scopes : attribute.Scopes;

            return new AzureAdTokenAttribute(tenantId, clientId, audience, roles, scopes);
        }
    }
}