using Domain.Messaging;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Messaging.RabbitMQ
{
    public class RabbitMQEventPublisher : IMessageBus
    {
        private readonly IModel _channel;

        public RabbitMQEventPublisher()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _channel = factory.CreateConnection().CreateModel();
        }

        public async Task PublishEventAsync<T>(T @event, CancellationToken cancellationToken) where T : class
        {
            var exchangeName = "MotorcycleExchange";
            var messageBody = JsonSerializer.Serialize(@event);
            var body = Encoding.UTF8.GetBytes(messageBody);

            _channel.ExchangeDeclare(exchange: exchangeName, type: "fanout");

            _channel.BasicPublish(
                exchange: exchangeName,
                routingKey: "",
                basicProperties: null,
                body: body
            );

            await Task.CompletedTask;
        }
    }
}
