using WF.Cqrs.Operations;

namespace WF.Cqrs.Handlers;

public interface ICommandHandler : IOperationHandler; //just for interface marking

public interface ICommandHandler<in TCommand, TResult> : IOperationHandler<TCommand, TResult>, ICommandHandler
    where TCommand : ICommand<TResult>
{
    // in theory, since I always used the `in` modifier in the TOperation parameter, I could omit all this hiding of methods.
    // But it allows me to have more significant names for parameters (i.e. "command" in place of "operation")
    ValueTask<TResult> IOperationHandler<TCommand, TResult>.HandleAsync(TCommand operation, CancellationToken cancellationToken)
        => HandleAsync(operation, cancellationToken);

    public new ValueTask<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken = default);
}

public interface ICommandHandler<in TCommand> : IOperationHandler<TCommand>, ICommandHandler
    where TCommand : ICommand
{
    ValueTask IOperationHandler<TCommand>.HandleAsync(TCommand operation, CancellationToken cancellationToken)
        => HandleAsync(operation, cancellationToken);

    public new ValueTask HandleAsync(TCommand command, CancellationToken cancellationToken);
}