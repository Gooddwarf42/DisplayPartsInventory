using Cqrs.Handlers;
using Cqrs.Operations;
using Microsoft.Extensions.DependencyInjection;
using Utils.Extensions;

namespace Cqrs.Mediator;

public class DefaultMediator(IServiceProvider serviceProvider, CqrsContext cqrsContext) : IMediator
{
    public ValueTask<TResult> RunAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        => RunOperationAsync(command, cancellationToken);

    public ValueTask<TResult> RunAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        => RunOperationAsync(query, cancellationToken);

    public ValueTask RunAsync(IEvent @event, CancellationToken cancellationToken = default)
        => throw new NotImplementedException(); // There is still something fishy in interfaces, I can't do it properly right now...

    private ValueTask<TResult> RunOperationAsync<TResult>(IOperation<TResult> operation, CancellationToken cancellationToken = default)
    {
        var operationType = operation.GetType();
        var resultType = typeof(TResult);
        // NOTE: we could restrict this to just ICommandHandler/QueryHandler/Whatever, but I don't think there is much to gain.
        var handlerInterfaceType = typeof(IOperationHandler<,>).MakeGenericType(operationType, resultType);

        var handlerImplementationType = cqrsContext.HandlerTypes
                                            .SingleOrDefault(type => type.Extends(handlerInterfaceType))
                                        ?? throw new ArgumentOutOfRangeException(nameof(operation), $"Command {operationType.Name} has no {nameof(IOperationHandler)} registered.");

        var handler = (IBaseOperationHandler<TResult>)serviceProvider.GetRequiredService(handlerImplementationType);
        return handler.HandleAsync(operation, cancellationToken);
    }
}