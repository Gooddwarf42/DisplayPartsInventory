using Cqrs.Operations;

namespace Cqrs.Mediator;

public class DefaultMediator : IMediator
{
    // TODO: to implement these, we need to have the handlers registered.
    // Thus, the DefaultMediator should know of its "CqrsContext" (or however you wanna call it).
    // containing information on all the hanled types (and other stuff potentially
    // such as decorators).
    // So the next goal is to make a ServiceCollectionExtension AddCqrs methog
    // that gathers all the handlers to register and registers the mediator
    // (the default mediator, if nothing is specified).
    
    public ValueTask<TResult> RunAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public ValueTask<TResult> RunAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();

    public ValueTask RunAsync(IEvent @event, CancellationToken cancellationToken = default)
        => throw new NotImplementedException();
}