using Cqrs.Operations;

namespace Cqrs.Handlers;

public interface IQueryHandler<TQuery, TResult> : IOperationHandler<TQuery>
    where TQuery : IQuery<TResult>
{
    public ValueTask<TResult> HandleAsync(TQuery query, CancellationToken cancellationToken);
}
