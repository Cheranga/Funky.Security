namespace Funky.Security.Api.Functions.Bindings
{
    internal class TokenConfig
    {
        public string Scopes { get; set; }
        public string Roles { get; set; }
        public string Audience { get; set; }
        public string TenantId { get; set; }
        public string ClientId { get; set; }
    }
}