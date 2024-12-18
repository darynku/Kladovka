
using Kladovka.Consumers.Managers;

namespace Notification
{
    public class HostingService : BackgroundService
    {
        private readonly IKafkaMessageConsumerManager _consumerManager;

        public HostingService(IKafkaMessageConsumerManager consumerManager)
        {
            _consumerManager = consumerManager;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _consumerManager.StartConsumers(stoppingToken);
            while(!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1, stoppingToken);
            }
        }
    }
}
