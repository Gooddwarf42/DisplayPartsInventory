using Cqrs.Operations;

namespace Cqrs.Handlers;
public interface IEventHandler : IOperationHandler; //just for interface marking

public interface IEventHandler<TEvent> : IOperationHandler<TEvent>, IEventHandler
    where TEvent : IEvent
{
    ValueTask IOperationHandler<TEvent>.HandleAsync(TEvent operation, CancellationToken cancellationToken)
        => HandleAsync(operation, cancellationToken);
    public new ValueTask HandleAsync(TEvent cqrsEvent, CancellationToken cancellationToken = default);
}
