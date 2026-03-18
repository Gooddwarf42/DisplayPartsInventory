using System.Collections.Concurrent;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using WF.Cqrs.Handlers;
using WF.Cqrs.Operations;
using WF.Utils.Extensions;

namespace WF.Cqrs.Mediator;

public class DefaultMediator(IServiceProvider serviceProvider, CqrsContext cqrsContext) : IMediator
{
    private static readonly ConcurrentDictionary<Type, Type> ImplementationTypes = new();

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
        var handlerImplementationType = GetHandlerImplementationType(operationType, resultType);

        var handler = (IBaseOperationHandler<TResult>)serviceProvider.GetRequiredService(handlerImplementationType);

        // Apply decorators
        handler = ApplyDecorators(handler, operationType);

        return handler.HandleAsync(operation, cancellationToken);
    }

    // TODO this probably should not be private. But I want to have it tested...
    // For now I'll put it as internal, so I can test it.
    // Consider refactoring this at some point. Or extracting this.
    internal Type GetHandlerImplementationType(Type operationType, Type resultType)
    {
        if (ImplementationTypes.TryGetValue(operationType, out var cachedImplementationType))
        {
            return cachedImplementationType;
        }

        if (!operationType.IsGenericType)
        {
            // NOTE: we could restrict this to just ICommandHandler/QueryHandler/Whatever, but I don't think there is much to gain.
            var handlerInterfaceType = typeof(IOperationHandler<,>).MakeGenericType(operationType, resultType);

            var handlerImplementationType = cqrsContext.HandlerTypes
                                                .Where(type => !type.IsGenericTypeDefinition)
                                                .SingleOrDefault(type => type.Extends(handlerInterfaceType))
                                            ?? throw new ArgumentOutOfRangeException(nameof(operationType), $"Command {operationType.Name} has no {nameof(IOperationHandler)} registered.");
            return handlerImplementationType;
        }

        var operationGenericTypeDefinition = operationType.GetGenericTypeDefinition();
        var operationGenericTypeArguments = operationType.GenericTypeArguments;

        var genericHandlerImplementationType = cqrsContext.HandlerTypes
                                                   .Where(type => type.IsGenericTypeDefinition)
                                                   .SingleOrDefault(MatchesGenericHandlerType)
                                               ?? throw new ArgumentOutOfRangeException(nameof(operationType), $"Command {operationType.Name} has no {nameof(IOperationHandler)} registered.");

        // TODO this relies on generics being in the same order in the Handler and the Operation. We should definitely improve this...
        var implementationType = genericHandlerImplementationType.MakeGenericType(operationGenericTypeArguments);

        ImplementationTypes.TryAdd(operationType, implementationType);

        return implementationType;

        bool MatchesGenericHandlerType(Type type)
        {
            var interfaces = type.GetInterfaces();
            var theInterface = interfaces
                .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IOperationHandler<,>));

            var interfaceTypeArguments = theInterface.GenericTypeArguments;
            var handlerOperationTypeArgument = interfaceTypeArguments[0];
            var handlerResultTypeArgument = interfaceTypeArguments[1];

            if (operationGenericTypeDefinition != handlerOperationTypeArgument.GetGenericTypeDefinition())
            {
                return false;
            }

            // We also need to check if the return type matches
            if (!resultType.IsGenericType)
            {
                if (handlerResultTypeArgument.IsGenericType)
                {
                    return false;
                }

                // This takes care of the case where the operation result is one of the generic arguments of the operation.
                if (handlerResultTypeArgument.IsGenericTypeParameter)
                {
                    return handlerResultTypeArgument.GenericParameterPosition >= operationGenericTypeArguments.Length
                        ? false
                        : operationGenericTypeArguments[handlerResultTypeArgument.GenericParameterPosition] == resultType;
                }

                return handlerResultTypeArgument == resultType;
            }

            // suppose it it List<PartSummaryDto> and the handler has a List<TSummaryDto> as return type
            var resultTypeGenericDefinition = resultType.GetGenericTypeDefinition(); // List<>
            var handlerResultTypeGenericDefinition = handlerResultTypeArgument.GetGenericTypeDefinition();

            // TODO I'll keep it simple for now. Should cover 99% of use cases, and only fail in some horrid ill maliciously formed OperationHandlers
            return handlerResultTypeGenericDefinition == resultTypeGenericDefinition;
        }
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
            (
                p =>
                    p.ParameterType.Extends(baseOperationHandlerType) // If the parameter is an IBaseOperationHandler<TResult>, it's my decoratee!
                        ? handler
                        : serviceProvider.GetRequiredService(p.ParameterType)
            )
            .ToArray();

        var decorator = (IBaseOperationHandler<TResult>)constructor.Invoke(constructorArguments);
        return decorator;
    }
}