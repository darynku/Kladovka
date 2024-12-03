using Confluent.Kafka;
using Kladovka.Consumers.Builders;
using Kladovka.Contracts.Abstract;
using System.Text.Json;

namespace Kladovka.Consumers.Consumers
{
    public abstract class BaseKafkaTopicConsumer<TEvent> : IKafkaTopicConsumer<TEvent> where TEvent : IEvent
    {
        private ILogger _logger;
        private IKafkaConsumerBuilder _consumerBuilder;
        public virtual string TopicName { get; } = typeof(TEvent).FullName!.ToLower();
        public BaseKafkaTopicConsumer(IKafkaConsumerBuilder consumerBuilder, ILogger logger)
        {
            _consumerBuilder = consumerBuilder;
            _logger = logger;
        }

        public abstract Task ProccessAsync(TEvent message, CancellationToken cancellationToken);
        public async Task StartConsuming(CancellationToken cancellationToken)
        {
            using var consumer = _consumerBuilder.Buid();
            consumer.Subscribe(TopicName);
            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = consumer.Consume(cancellationToken) ?? throw new Exception($"null result message from {TopicName}");

                        var message = JsonSerializer.Deserialize<TEvent>(consumeResult.Message.Value) ?? throw new Exception($"null message from {TopicName}");

                        await ProccessAsync(message, cancellationToken);
                    }
                    catch (ConsumeException ex)
                    {

                        _logger.LogError(ex, "cant get message from topic {TopicName}", TopicName);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "occurred exception while try getting message from topic {TopicName}", TopicName);
                    }
                }
            }
            catch (OperationCanceledException ex)
            {
                _logger.LogError(ex, "Operation was cancelled");
            }
            finally
            {
                consumer.Close();
            }

        }
    }
}
