using Confluent.Kafka;

namespace Kladovka.Consumers.Builders
{
    public interface IKafkaConsumerBuilder
    {
        IConsumer<string, string> Buid();
    }
}