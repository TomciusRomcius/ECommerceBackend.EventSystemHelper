namespace EventSystemHelper.Interfaces
{
    public interface IEventConsumser
    {
        public T? Consume<T>(CancellationToken cancellationToken);
    }
}
