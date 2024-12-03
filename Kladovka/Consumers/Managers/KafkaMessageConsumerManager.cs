using Kladovka.Consumers.Consumers;

namespace Kladovka.Consumers.Managers
{
    public class KafkaMessageConsumerManager : IKafkaMessageConsumerManager
    {
        private readonly IEnumerable<IKafkaTopicConsumer> _consumers;

        public KafkaMessageConsumerManager(IEnumerable<IKafkaTopicConsumer> consumers)
        {
            _consumers = consumers;
        }

        public void StartConsumers(CancellationToken cancellationToken)
        {
            foreach (var consumer in _consumers)
            {
                new Thread(() =>
                    consumer.StartConsuming(cancellationToken)).Start();
            }
        }
    }
}
