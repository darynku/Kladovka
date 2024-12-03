using Confluent.Kafka;
using Kladovka.Consumers.Options;
using Microsoft.Extensions.Options;

namespace Kladovka.Consumers.Builders
{
    public class KafkaConsumerBuilder : IKafkaConsumerBuilder
    {
        private readonly KafkaOptions _options;
        public KafkaConsumerBuilder(IOptions<KafkaOptions> options)
        {
            _options = options.Value;
        }
        public IConsumer<string, string> Buid()
        {
            var config = new ClientConfig
            {
                BootstrapServers = _options.BootstrapServers,
                AllowAutoCreateTopics = true
            };

            var consumerConfig = new ConsumerConfig(config)
            {
                AutoOffsetReset = AutoOffsetReset.Earliest,
                AllowAutoCreateTopics = true
            };

            var consumerConfigBuilder = new ConsumerBuilder<string, string>(consumerConfig);

            return consumerConfigBuilder.Build();
        }
    }
}
