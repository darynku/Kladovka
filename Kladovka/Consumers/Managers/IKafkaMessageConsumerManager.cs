
namespace Kladovka.Consumers.Managers
{
    public interface IKafkaMessageConsumerManager
    {
        void StartConsumers(CancellationToken cancellationToken);
    }
}