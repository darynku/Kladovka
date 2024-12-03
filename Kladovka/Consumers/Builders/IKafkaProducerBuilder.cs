using Confluent.Kafka;

namespace Kladovka.Consumers.Builders
{
    public interface IKafkaProducerBuilder
    {
        IProducer<Null, string> Build();
    }
}