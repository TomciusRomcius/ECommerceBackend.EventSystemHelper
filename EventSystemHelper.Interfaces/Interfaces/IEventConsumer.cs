namespace EventSystemHelper.Interfaces.Interfaces
{
    public interface IEventConsumser
    {
        public T? Consume<T>(CancellationToken cancellationToken);
    }
}
