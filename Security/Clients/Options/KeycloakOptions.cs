namespace Security.Clients.Options
{
    public class KeycloakOptions
    {
        public string ClientUuid { get; init; } = string.Empty;
        public string Authority { get; init; } = string.Empty;
        public string AuthorizationUrl { get; init; } = string.Empty;
        public string ClientId { get; init; } = string.Empty;
        public string ClientSecret { get; init; } = string.Empty;
        public string MetadataAddress { get; init; } = string.Empty;
        public string Realm { get; init; } = string.Empty;
        public string ApiAdminBaseUrl { get; init; } = string.Empty;
        public string ApiClientId { get; init; } = string.Empty;
        public bool RequireHttpsMetadata { get; init; }
        public string ApiClientSecret { get; init; } = string.Empty;
    }
}
