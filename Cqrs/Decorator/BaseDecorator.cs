using Cqrs.Handlers;
using Cqrs.Operations;

namespace Cqrs.Decorator;

public interface IDecorator;

public abstract class BaseDecorator<TOperation, TResult>(IOperationHandler<TOperation, TResult> decoratee) : IOperationHandler<TOperation, TResult>, IDecorator
    where TOperation : IOperation<TResult>
{
    public ValueTask<TResult> HandleAsync(TOperation operation, CancellationToken cancellationToken = default)
        => DecorateAsync(decoratee, operation, cancellationToken);

    protected abstract ValueTask<TResult> DecorateAsync(IOperationHandler<TOperation, TResult> decoratee, TOperation operation, CancellationToken cancellationToken);
}