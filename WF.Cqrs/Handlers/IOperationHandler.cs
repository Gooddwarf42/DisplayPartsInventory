using WF.Cqrs.Operations;

namespace WF.Cqrs.Handlers;

public interface IOperationHandler; // Just for interface marking

public interface IBaseOperationHandler<TResult> : IOperationHandler // I need this middle class to write the DefaultMediator!
{
    public ValueTask<TResult> HandleAsync(IOperation<TResult> operation, CancellationToken cancellationToken = default);
}

public interface IOperationHandler<in TOperation, TResult> : IBaseOperationHandler<TResult>
    where TOperation : IOperation<TResult>
{
    ValueTask<TResult> IBaseOperationHandler<TResult>.HandleAsync(IOperation<TResult> operation, CancellationToken cancellationToken)
        => HandleAsync((TOperation)operation, cancellationToken);

    public ValueTask<TResult> HandleAsync(TOperation operation, CancellationToken cancellationToken = default);
}

public interface IOperationHandler<in TOperation> : IOperationHandler<TOperation, Empty>
    where TOperation : IOperation
{
    async ValueTask<Empty> IOperationHandler<TOperation, Empty>.HandleAsync(TOperation operation, CancellationToken cancellationToken)
    {
        await HandleAsync(operation, cancellationToken);
        return default;
    }

    public new ValueTask HandleAsync(TOperation operation, CancellationToken cancellationToken = default);
}