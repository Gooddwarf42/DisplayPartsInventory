using Cqrs.Operations;

namespace Cqrs.Mediator;

public class DefaultMediator : IMediator
{
    public ValueTask<TResult> RunAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public ValueTask<TResult> RunAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public ValueTask RunAsync(IEvent cqrsEvent, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}