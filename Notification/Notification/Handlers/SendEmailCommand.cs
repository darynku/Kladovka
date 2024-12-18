using Kladovka.Contracts.Abstract;
using Notification.Contracts;

namespace Notification.Handlers
{
    public class SendEmailCommand : Command<Response>
    {
        public SendEmailCommand(EmailNotificationData data)
        {
            Email = data.Email;
        }
        public string Email { get; }
    }
}
