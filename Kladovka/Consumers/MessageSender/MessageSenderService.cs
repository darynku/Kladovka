using Confluent.Kafka;
using Kladovka.Consumers.Builders;
using Kladovka.Contracts.Abstract;
using System.Text.Json;

namespace Kladovka.Consumers.MessageSender
{
    public class MessageSenderService : IDisposable, IMessageSenderService
    {
        private readonly Lazy<IProducer<Null, string>> _producer;

        public MessageSenderService(IKafkaProducerBuilder kafkaProducerBuilder)
        {
            _producer = new Lazy<IProducer<Null, string>>(kafkaProducerBuilder.Build());
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        public virtual void Dispose(bool disposing)
        {
            if (disposing && _producer.IsValueCreated)
            {
                _producer.Value.Dispose();
            }
        }

        public async Task SendAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
            where TEvent : class, IEvent
        {
            var json = JsonSerializer.Serialize(@event);
            var topic = @event.GetType().FullName!.ToLower();

            var message = new Message<Null, string> { Value = json };

            await _producer.Value.ProduceAsync(topic, message, cancellationToken);
            _producer.Value.Flush(cancellationToken);
        }
    }
}
