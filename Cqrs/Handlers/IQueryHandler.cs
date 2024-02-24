using Cqrs.Operations;

namespace Cqrs.Handlers;
public interface IQueryHandler : IOperationHandler; //just for interface marking

public interface IQueryHandler<TQuery, TResult> : IOperationHandler<TQuery, TResult>, IQueryHandler
    where TQuery : IQuery<TResult>
{
    ValueTask<TResult> IOperationHandler<TQuery, TResult>.HandleAsync(TQuery operation, CancellationToken cancellationToken)
        => HandleAsync(operation, cancellationToken);
    public new ValueTask<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken = default);
}
