using Kladovka.Contracts.Abstract;

namespace Kladovka.Contracts.Handlers
{
    public interface IEventHandler
    {
        public Task HandleAsync(IEvent @event, CancellationToken cancellationToken);
    }

    public abstract class EventHandler<TEvent> : IEventHandler
        where TEvent : IEvent
    {
        public Task HandleAsync(IEvent @event, CancellationToken cancellationToken)
        {
            return HandleAsync((TEvent)@event, cancellationToken);
        }

        public abstract Task HandleAsync(TEvent @event, CancellationToken cancellationToken);
    }
}
