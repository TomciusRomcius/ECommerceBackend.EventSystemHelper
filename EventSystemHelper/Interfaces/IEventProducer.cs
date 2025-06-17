namespace EventSystemHelper.Interfaces
{
    public interface IEventProducer
    {
        Task ProduceEventAsync(string topic, string jsonMessage, CancellationToken cancellationToken);
    }
}
