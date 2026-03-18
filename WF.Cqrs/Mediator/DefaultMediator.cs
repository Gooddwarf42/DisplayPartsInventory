using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using WF.Cqrs.Handlers;
using WF.Cqrs.Operations;
using WF.Cqrs.Services;
using WF.Utils.Extensions;

namespace WF.Cqrs.Mediator;

public class DefaultMediator(IServiceProvider serviceProvider, CqrsContext cqrsContext, OperationHandlerResolver operationHandlerResolver) : IMediator
{
    private static readonly ConcurrentDictionary<Type, Type> ImplementationTypes = new();

    public ValueTask<TResult> RunAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
        => RunOperationAsync(command, cancellationToken);

    public ValueTask<TResult> RunAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
        => RunOperationAsync(query, cancellationToken);

    public async ValueTask RunAsync(IEvent @event, CancellationToken cancellationToken = default)
        => await RunOperationAsync(@event, cancellationToken);

    private ValueTask<TResult> RunOperationAsync<TResult>(IOperation<TResult> operation, CancellationToken cancellationToken = default)
    {
        var operationType = operation.GetType();
        var handlerImplementationType = GetHandlerImplementationType(operationType);

        var handler = (IBaseOperationHandler<TResult>) serviceProvider.GetRequiredService(handlerImplementationType);

        // Apply decorators
        handler = ApplyDecorators(handler, operationType);

        return handler.HandleAsync(operation, cancellationToken);
    }

    private Type GetHandlerImplementationType(Type operationType)
    {
        if (ImplementationTypes.TryGetValue(operationType, out var cachedImplementationType))
        {
            return cachedImplementationType;
        }

        var implementationType = operationHandlerResolver.GetOperationHandlerImplementationType(operationType, cqrsContext.HandlerTypes);
        ImplementationTypes.TryAdd(operationType, implementationType);
        return implementationType;
    }

    private IBaseOperationHandler<TResult> ApplyDecorators<TResult>(IBaseOperationHandler<TResult> handler, Type operationType)
    {
        foreach (var decoratorType in cqrsContext.GetDecoratorsTypes(operationType).Reverse())
        {
            // decoratorType extends BaseDecorator<TOperation, TResult>. We need to close these generics
            // NOTE this relies on the concrete decorator class having generics in the proper order... i don't really like this. TODO can we improve?

            var actualDecoratorType = decoratorType.MakeGenericType(operationType, typeof(TResult));
            handler = ApplyDecorator(actualDecoratorType, handler);
        }

        return handler;
    }

    private IBaseOperationHandler<TResult> ApplyDecorator<TResult>(Type actualDecoratorType, IBaseOperationHandler<TResult> handler)
    {
        var constructors = actualDecoratorType.GetConstructors(BindingFlags.Public | BindingFlags.Instance);

        if (constructors.Length != 1)
        {
            throw new ArgumentOutOfRangeException(nameof(actualDecoratorType), $"The decorator type {actualDecoratorType} should have exactly one public constructor!");
        }

        var constructor = constructors[0];

        var constructorParameters = constructor.GetParameters();

        // Resolve parameters from ServiceProvider, so we can use dependency injection on decorators too
        var baseOperationHandlerType = typeof(IBaseOperationHandler<TResult>);

        var constructorArguments = constructorParameters
            .Select
            (p =>
                p.ParameterType.Extends(baseOperationHandlerType) // If the parameter is an IBaseOperationHandler<TResult>, it's my decoratee!
                    ? handler
                    : serviceProvider.GetRequiredService(p.ParameterType)
            )
            .ToArray();

        var decorator = (IBaseOperationHandler<TResult>) constructor.Invoke(constructorArguments);
        return decorator;
    }
}
