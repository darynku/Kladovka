using Kladovka.Contracts.Abstract;

namespace Kladovka.Consumers.MessageSender
{
    public interface IMessageSenderService
    {
        void Dispose();
        void Dispose(bool disposing);
        Task SendAsync<TEvent>(TEvent @event, CancellationToken cancellationToken) where TEvent : class, IEvent;
    }
}