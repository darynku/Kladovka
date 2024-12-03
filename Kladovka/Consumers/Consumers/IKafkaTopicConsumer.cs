using Kladovka.Contracts.Abstract;

namespace Kladovka.Consumers.Consumers
{
    public interface IKafkaTopicConsumer<in TEvent> : IKafkaTopicConsumer
        where TEvent : IEvent
    {
        Task ProccessAsync(TEvent message, CancellationToken cancellationToken);
        
    }
    public interface IKafkaTopicConsumer
    {
        string TopicName { get; }

        Task StartConsuming(CancellationToken cancellationToken);
    }
}