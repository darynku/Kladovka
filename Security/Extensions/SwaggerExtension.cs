using Microsoft.OpenApi.Models;

namespace Security.Extensions
{
    public static class SwaggerExtension
    {
        public static IServiceCollection AddSwaggerOAuth2(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddSwaggerGen(o =>
            {
                o.CustomSchemaIds(id => id.FullName?.Replace("+", "-"));

                o.AddSecurityDefinition("Keycloak", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri(configuration["Keycloak:AuthorizationUrl"]!),
                            Scopes = new Dictionary<string, string>
                            {
                                { "openid", "openid" },
                                { "profile", "profile" }
                            }
                        }
                    }
                });
               

                var securityReq = new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = "Keycloak",
                                Type = ReferenceType.SecurityScheme
                            },
                            In = ParameterLocation.Header,
                            Name = "Bearer",
                            Scheme = "Bearer"
                        },
                        []
                    }
                };

                o.AddSecurityRequirement(securityReq);
            });

            

            return services;
        
        }
    }
}
