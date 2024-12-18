
namespace Notification.Services
{
    public interface IEmailSendingService
    {
        Task SendEmail(string email, string title, string message, CancellationToken cancellationToken);
    }
}