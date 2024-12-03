using Confluent.Kafka;
using Kladovka.Consumers.Options;
using Microsoft.Extensions.Options;

namespace Kladovka.Consumers.Builders
{
    public class KafkaProducerBuilder : IKafkaProducerBuilder
    {
        private readonly KafkaOptions _options;

        public KafkaProducerBuilder(IOptions<KafkaOptions> options)
        {
            _options = options.Value;
        }

        public IProducer<Null, string> Build()
        {
            var config = new ClientConfig
            {
                BootstrapServers = _options.BootstrapServers
            };

            var producerBuilder = new ProducerBuilder<Null, string>(config);

            return producerBuilder.Build();
        }
    }
}
