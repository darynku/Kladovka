using Notification.Contracts;

namespace Notification.Clients
{
    public interface IEmailNotificationClient
    {
        Task SendEmailAsync(EmailNotificationData data, CancellationToken cancellationToken);
    }
}