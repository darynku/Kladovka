namespace Kladovka.Contracts.Abstract
{
    public class Event : IEvent
    {
        public Guid CorrelationId { get; set; }
        public Event()
        {
            CorrelationId = Guid.Empty;
        }
        public Event(Guid correlationId)
        {
            CorrelationId = correlationId;
        }
    }
    public interface IEvent : IMessage
    {
    }

}
