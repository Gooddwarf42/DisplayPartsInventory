using Cqrs.Operations;

namespace Cqrs.Handlers;

public interface IOperationHandler; // Just for interface marking

public interface IOperationHandler<in TOperation, TResult> : IOperationHandler
    where TOperation : IOperation<TResult>
{
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