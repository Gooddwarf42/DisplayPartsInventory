using Cqrs.Operations;

namespace Cqrs.Handlers;
// TODO: this is still very much a draft!
public interface ICommandHandler<TCommand, TResult> : IOperationHandler
    where TCommand : ICommand<TResult>
{
    public ValueTask<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}

public interface ICommandHandler<TCommand> : ICommandHandler<TCommand, Empty>
    where TCommand : ICommand
{
    async ValueTask<Empty> ICommandHandler<TCommand, Empty>.HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        await HandleAsync(command, cancellationToken);
        return default;
    }

    public new ValueTask HandleAsync(TCommand command, CancellationToken cancellationToken);
}
