namespace Eciton.Application.Abstractions;
public interface IEventHandler<TEvent>
{
    Task HandleAsync(TEvent @event);
}
