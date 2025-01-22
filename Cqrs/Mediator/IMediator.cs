using Cqrs.Operations;

namespace Cqrs.Mediator;

public interface IMediator
{
    public ValueTask<TResult> RunAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default);

    public async ValueTask RunAsync(ICommand command, CancellationToken cancellationToken = default)
        => await RunAsync<Empty>(command, cancellationToken);

    public ValueTask<TResult> RunAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default);

    public ValueTask RunAsync(IEvent @event, CancellationToken cancellationToken = default);
}