using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMQConsumerHostedService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public RabbitMQConsumerHostedService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var consumer = scope.ServiceProvider.GetRequiredService<RabbitMQEventConsumer>();
                consumer.StartConsuming();
                await Task.Delay(Timeout.Infinite, stoppingToken);
            }
        }
    }
}
