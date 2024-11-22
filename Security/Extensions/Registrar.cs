using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Security.Applications.AppServices.Identity.Services;
using Security.Applications.AppServices.Identity.Services.Abstract;
using Security.Applications.Handlers.Identity.Commands.Login;
using Security.Clients.Keycloack;
using Security.Clients.Keycloak.Generated;
using Security.Clients.Options;
using System.Reflection;

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

            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.Audience = kcOptions.Audience;
                    o.MetadataAddress = kcOptions.MetadataAddress;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidIssuer = kcOptions.Issuer
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
        public static IServiceCollection AddClients(this IServiceCollection services)
        {
            services.AddScoped<IKeycloakAuthorizationApi, KeycloakAuthorizationApi>();

            services.AddHttpClient<IKeycloakGeneratedExternalApiClient, KeycloakGeneratedExternalApiClient>(
                              client => client.BaseAddress = new Uri("http://keycloak:8080"));
            return services;
        }

    }
}
