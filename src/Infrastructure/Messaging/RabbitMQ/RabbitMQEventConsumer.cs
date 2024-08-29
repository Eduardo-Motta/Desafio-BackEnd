using Domain.Repositories;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMQEventConsumer
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IModel _channel;

        public RabbitMQEventConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var factory = new ConnectionFactory() { HostName = "localhost" };
            _channel = factory.CreateConnection().CreateModel();
        }

        public void StartConsuming()
        {
            var exchangeName = "MotorcycleExchange";
            var queueName = "Motorcycle_2024";

            _channel.ExchangeDeclare(exchange: exchangeName, type: "fanout");
            _channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            _channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");


            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var motorcycle = JsonSerializer.Deserialize<MotorcycleEntity>(message);

                if (motorcycle != null && motorcycle.Year == 2024)
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var repository = scope.ServiceProvider.GetRequiredService<IMotorcycleNotificationRepository>();

                        var notification = new MotorcycleNotificationEntity(
                            Guid.NewGuid(),
                            motorcycle.Model,
                            motorcycle.Year,
                            motorcycle.LicensePlate
                        );

                        await repository.CreateMotorcycleNotification(notification, CancellationToken.None);
                    }
                }
            };

            _channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
        }
    }
}
