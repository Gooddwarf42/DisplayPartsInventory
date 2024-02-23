using Cqrs.Operations;

namespace Cqrs.Handlers;

public interface IQueryHandler<TQuery, TResult> : IOperationHandler
    where TQuery : IQuery<TResult>
{
    public ValueTask<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
