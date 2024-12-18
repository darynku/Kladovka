using Kladovka.Contracts.Abstract;

namespace Notification.Contracts.Events
{
    public class EmailNotificationEvent : Event
    {
        public EmailNotificationEvent(
            Guid correlationId,
            string email) : base(correlationId)
        {
            Email = email;
        }
        public string Email { get; set; }   
    }
}
