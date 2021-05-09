using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;

namespace Funky.Security.Api.Functions.Bindings
{
    public class AzureAdTokenValidationService
    {
        private const string Authorization = nameof(Authorization);
        private const string Bearer = nameof(Bearer);

        private const string ScopeType = @"http://schemas.microsoft.com/identity/claims/scope";
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AzureAdTokenValidationService> _logger;
        private readonly ISecurityTokenValidator _securityTokenValidator;

        public AzureAdTokenValidationService(IHttpContextAccessor httpContextAccessor, ISecurityTokenValidator securityTokenValidator, ILogger<AzureAdTokenValidationService> logger)
        {
            _httpContextAccessor = httpContextAccessor;
            _securityTokenValidator = securityTokenValidator;
            _logger = logger;
        }

        public async Task<ClaimsPrincipal> GetClaimsPrincipalAsync(AzureAdTokenAttribute azureAdData)
        {
            if (!_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(Authorization, out var headerData))
            {
                return null;
            }

            var headerValue = headerData.FirstOrDefault();
            if (headerValue == null)
            {
                return null;
            }

            if (!headerValue.Contains(Bearer, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var token = headerValue.Substring($"{Bearer} ".Length);

            if (string.IsNullOrEmpty(token))
            {
                return null;
            }

            try
            {
                var openIdConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(azureAdData.AuthorizeUrl, new OpenIdConnectConfigurationRetriever());
                var openIdConnectConfigData = await openIdConfigurationManager.GetConfigurationAsync();

                var validationParameters = new TokenValidationParameters
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    RequireSignedTokens = true,
                    ClockSkew = TimeSpan.Zero,
                    
                    ValidAudience = azureAdData.Audience,

                    IssuerSigningKeys = openIdConnectConfigData.SigningKeys,
                    ValidIssuer = openIdConnectConfigData.Issuer
                };

                var principal = _securityTokenValidator.ValidateToken(token, validationParameters, out var securityToken);

                var requiredScopes = azureAdData.Scopes?.Replace(" ", string.Empty)?.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)?.ToList();
                var requiredRoles = azureAdData.Roles?.Replace(" ", string.Empty)?.Split(new[] {","}, StringSplitOptions.RemoveEmptyEntries)?.ToList();

                var isValid = IsValid(principal, requiredScopes, requiredRoles);

                return isValid ? principal : null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error when validating the user token");
            }

            return null;
        }


        private bool IsValid(ClaimsPrincipal claimsPrincipal, List<string> requiredScopes = null, List<string> requiredRoles = null)
        {
            if (claimsPrincipal == null)
            {
                return false;
            }

            requiredScopes = requiredScopes?.ToList() ?? new List<string>();
            requiredRoles = requiredRoles?.ToList() ?? new List<string>();

            if (!requiredScopes.Any() && !requiredRoles.Any())
            {
                return true;
            }

            var hasAccessToRoles = false;
            var hasAccessToScopes = false;

            if (requiredRoles.Any())
            {
                hasAccessToRoles = requiredRoles.All(claimsPrincipal.IsInRole);
            }

            if (requiredScopes.Any())
            {
                var scopeClaim = claimsPrincipal.HasClaim(x => x.Type == ScopeType)
                    ? claimsPrincipal.Claims.First(x => x.Type == ScopeType).Value
                    : string.Empty;

                var tokenScopes = scopeClaim?.Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries)?.ToList() ?? new List<string>();
                hasAccessToScopes = requiredScopes.All(x => tokenScopes.Any(y => string.Equals(x, y, StringComparison.OrdinalIgnoreCase)));
            }

            return hasAccessToRoles && hasAccessToScopes;
        }
    }
}