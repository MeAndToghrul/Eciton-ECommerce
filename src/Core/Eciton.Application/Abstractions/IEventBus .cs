namespace Eciton.Application.Abstractions;
public interface IEventBus
{
    Task PublishAsync<TEvent>(TEvent @event) where TEvent : class;
}
