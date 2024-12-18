using Kladovka.Consumers.Builders;
using Kladovka.Consumers.Consumers;
using Notification.Clients;
using Notification.Contracts;
using Notification.Contracts.Events;
using Notification.Services;

namespace Notification.Consumers
{
    public class EmailNotificationConsumer : BaseKafkaTopicConsumer<EmailNotificationEvent>
    {
        private readonly IEmailNotificationClient _emailNotificationClient;
        public EmailNotificationConsumer(
            IKafkaConsumerBuilder consumerBuilder,
            ILogger<EmailNotificationConsumer> logger,
            IEmailNotificationClient emailNotificationClient) : base(consumerBuilder, logger)
        {
            _emailNotificationClient = emailNotificationClient;
        }

        public override Task ProccessAsync(EmailNotificationEvent message, CancellationToken cancellationToken)
        {
            var data = new EmailNotificationData()
            {
                Email = message.Email,
            };

            _emailNotificationClient.SendEmailAsync(data, cancellationToken);
            return Task.CompletedTask;
        }
    }
}
