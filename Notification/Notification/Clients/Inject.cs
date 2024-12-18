using Notification.Options;

namespace Notification.Clients
{
    public static class Inject
    {
        public static IServiceCollection AddNotificationClient(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .Configure<NotificationClientOptions>(configuration.GetSection("NotificationClient"))
                .AddHttpClient<IEmailNotificationClient, EmailNotificationClient>();

            return services;
        }
    }
}
