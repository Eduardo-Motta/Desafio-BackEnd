namespace Domain.Messaging
{
    public interface IMessageBus
    {
        Task PublishEventAsync<T>(T @event, CancellationToken cancellationToken) where T : class;
    }
}
