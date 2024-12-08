using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Security.Applications.AppServices.Identity.Services;
using Security.Applications.AppServices.Identity.Services.Abstract;
using Security.Clients.Keycloack;
using Security.Clients.Keycloak.Generated;
using Security.Clients.Options;
using System.Reflection;
using IdentityModel.Client;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;



namespace Security.Extensions
{
    public static class Registrar
    {
        public static IServiceCollection AddKeycloak(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var kcOptions = configuration.GetRequiredSection("Keycloak").Get<KeycloakOptions>()
                ?? throw new ArgumentNullException("Не удалось найти конфигурацию для keyclock");


            services.AddHttpClient();
            services.AddHttpContextAccessor();

            JsonWebTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.Authority = kcOptions.Authority;
                    o.RequireHttpsMetadata = kcOptions.RequireHttpsMetadata;
                    o.Audience = kcOptions.ClientId;
                    o.MetadataAddress = kcOptions.MetadataAddress;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidAudience = kcOptions.ClientId,
                    };
                });
            services.AddAuthorization();

            services.AddScoped<IAuthorizationService, AuthorizationService>();

            services.Configure<KeycloakOptions>(configuration.GetSection("Keycloak"));

            return services;
        }

        public static IServiceCollection AddHandlers(this IServiceCollection services)
        {
            var currentAssembly = Assembly.GetExecutingAssembly();
            services.AddMediatR(config => config.RegisterServicesFromAssembly(currentAssembly));
            return services;
        }
        public static IServiceCollection AddClients(this IServiceCollection services, IConfiguration configuration)
        {

            var optionsSection = configuration.GetSection("Keycloak")
                ?? throw new ArgumentNullException("Keycloak configuration is null");
            var config = new KeycloakOptions();
            optionsSection.Bind(config);

            services.AddAccessTokenManagement(options =>
            {
                options.Client.CacheLifetimeBuffer = 0;
                options.Client.Clients.Add("Keycloak", CreateClientCredentialsTokenRequest(config));
            });


            services.AddScoped<IKeycloakAuthorizationApi, KeycloakAuthorizationApi>();

            services.AddHttpClient<IKeycloakGeneratedExternalApiClient, KeycloakGeneratedExternalApiClient>(
                              client =>
                              {
                                  client.BaseAddress = new Uri(config.ApiAdminBaseUrl 
                                      ?? throw new ArgumentNullException("ApiAdminBaseUrl not found"));
                              }).AddClientAccessTokenHandler("Keycloak");

            services.AddScoped(provider => new Lazy<IKeycloakAuthorizationApi>(() =>
                 new KeycloakAuthorizationApi(
            provider.GetRequiredService<IHttpClientFactory>(),
            provider.GetRequiredService<IOptions<KeycloakOptions>>(),
            provider.GetRequiredService<ILogger<KeycloakAuthorizationApi>>(),
            provider.GetRequiredService<IKeycloakGeneratedExternalApiClient>())));

            services.AddScoped(provider => new Lazy<>)

            return services;
        }

        public static ClientCredentialsTokenRequest CreateClientCredentialsTokenRequest(KeycloakOptions options)
        {
            return new ClientCredentialsTokenRequest
            {
                Address = $"{options.ApiAdminBaseUrl}/realms/master/protocol/openid-connect/token",
                ClientId = options.ApiClientId,
                ClientSecret = options.ApiClientSecret,
                ClientCredentialStyle = ClientCredentialStyle.PostBody
            };
        }
    }
}
