using Kladovka.Consumers.Builders;
using Kladovka.Consumers.Consumers;
using Kladovka.Consumers.Managers;
using Kladovka.Consumers.Options;
using Kladovka.Contracts.Handlers.Extensions;
using Kladovka.Cqrs;
using Notification.Clients;
using Notification.Consumers;
using Notification.Controllers;
using Notification.Handlers;
using Notification.Services;
using System.Reflection;

namespace Notification
{
    public static class Inject
    {
        public static IServiceCollection RegistrarServices(this IServiceCollection services, IConfiguration configuration)
        {
            var currentAssembly = typeof(Program).Assembly;
            services.
                AddServices()
                .AddHandlers(currentAssembly)
                .AddKafkaServices();

            return services;
        }
        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.
                AddScoped<IEmailSendingService, EmailSendingService>(); 
            return services;
        }        
        public static IServiceCollection AddHandlers(this IServiceCollection services, Assembly currentAssembly)
        {
            services
                .AddMediatRExtension(currentAssembly)
                .AddRequestsHandlers(typeof(SendEmailCommandHandler).Assembly); 
            return services;
        }

        public static IServiceCollection AddKafkaServices(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddHostedService<HostingService>()
                .AddTransient<IKafkaMessageConsumerManager, KafkaMessageConsumerManager>()
                .AddTransient<IKafkaConsumerBuilder, KafkaConsumerBuilder>()
                .AddTransient<IKafkaTopicConsumer, EmailNotificationConsumer>()
                .AddNotificationClient(configuration)
                .Configure<KafkaOptions>(configuration.GetSection("Kafka"));
            return services;
        }
    }
}
