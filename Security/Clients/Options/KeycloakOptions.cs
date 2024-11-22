namespace Security.Clients.Options
{
    public class KeycloakOptions
    {
        public string Authority { get; init; } = string.Empty;
        public string Issuer { get; init; } = string.Empty;
        public string Audience { get; init; } = string.Empty;
        public string AuthorizationUrl { get; init; } = string.Empty;
        public string ClientId { get; init; } = string.Empty;
        public string ClientSecret { get; init; } = string.Empty;
        public string MetadataAddress { get; init; } = string.Empty;
        public string Realm { get; init; } = string.Empty;
    }
}
