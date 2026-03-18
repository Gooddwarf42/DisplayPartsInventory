using System.Reflection;
using WF.Cqrs.Handlers;
using WF.Cqrs.Operations;
using WF.Utils.Extensions;

namespace WF.Cqrs.Services;

// Mi piacerebbe che non fosse public, vorrei più o meno nasconderlo...
public sealed class OperationHandlerResolver
{
    public Type GetOperationHandlerImplementationType(Type operationType, IEnumerable<Type> handlerCollection)
    {
        if (!operationType.Extends<IBaseOperation>())
        {
            throw new ArgumentException($"Operation type {operationType} shoudl extend {nameof(IBaseOperation)}", nameof(operationType));
        }

        var resultType = GetResultType(operationType);

        if (!operationType.IsGenericType) // This implies the result type can not be generic too
        {
            var handlerInterfaceType = typeof(IOperationHandler<,>).MakeGenericType(operationType, resultType);

            var handlerImplementationType = handlerCollection
                    .Where(type => !type.IsGenericTypeDefinition)
                    .SingleOrDefault(type => type.Extends(handlerInterfaceType))
                ?? throw new ArgumentOutOfRangeException(nameof(operationType), $"Operation {operationType.Name} has no {nameof(IOperationHandler)} registered.");

            return handlerImplementationType;
        }

        var operationGenericTypeDefinition = operationType.GetGenericTypeDefinition();
        var operationGenericTypeArguments = operationType.GenericTypeArguments;

        var genericHandlerImplementationType = handlerCollection
                .Where(type => type.IsGenericTypeDefinition)
                .SingleOrDefault(type => GetHandlerOperationTypeArgument(type).GetGenericTypeDefinition() == operationGenericTypeDefinition)
            ?? throw new ArgumentOutOfRangeException(nameof(operationType), $"Command {operationType.Name} has no {nameof(IOperationHandler)} registered.");

        // Since in the IOperationHandler<TOperation<Parameters>, TResult<Parameters>> the result type is
        // fully determined (by compile time constraints) by the OperationType, the set of generic parameters of the result can
        // not contain things that ar not included in the set of generic parameters of the operation.
        // Thus, we infer that the actual OperationHandlerImplementation takes a subset of parameters from the
        // parameters used in the closed operation interface, potentially not all of them.

        // To clarify things, let us avoid conflicts in names, and think about this example
        // MyOperation<T,S> : IOperation<S>
        // MyOperationHandler<U> : IOperationHandler<MyOperation<int, U>, U>).

        // When someone wants to resolve the handler for, say, MyOperation<char, float>, we detected that
        // MyOperationHandler<U> is the generic definition we need to use to resolve the handler.

        // We need to figure out how to pass the right parameter as U (in this case, it would be float).
        // To do so, I need to know specifically the IOperationHandler interface my handler implements
        // (in this case IOperationHandler<MyOperation<int, U>, U>)
        // so I can infer how to map the genericTypeArguments of the operationType into the genericTypeParameters
        // needed to make the genericImplementationType of the handler.
        // In this case, I know I need to replace the *second* argument of the operation type (thus float)
        // for the U parameter of the handler.

        // In the example discussed above, this method takesIOperationHandler<MyOperation<int, U>, U>
        // and extracts the closed type argument of the operation, that is  MyOperation<int, U>
        var handlerOperationTypeArgument = GetHandlerOperationTypeArgument(genericHandlerImplementationType);

        var genericHandlerImplementationTypeParameters = ((TypeInfo) genericHandlerImplementationType).GenericTypeParameters; // [U]
        // OK, this U is a type parameter which closes the generic in the interface, so its GenericParameterPosition is 0
        // since it is taken with resppect to the MyOperationHandler<U> type (genericHandlerImplementationType).

        // these would be [(0, int) (1 U)]
        var typeArgumentsWithPosition = handlerOperationTypeArgument
            .GenericTypeArguments
            .Select((t, i) => new {Index = i, TypeArgument = t});

        Type[] typesToUseToCloseTheGeneric = genericHandlerImplementationTypeParameters
            .Select(handlerTypeParameter =>
            {
                // in the only iteration of the example, this would be U with position 1 in the MyOperation<int, U>
                var typeArgumentToUse = typeArgumentsWithPosition
                    .SingleOrDefault(e => e.TypeArgument == handlerTypeParameter);

                // operationGenericTypeArguments is [char, float]
                return typeArgumentToUse is null
                    ? null
                    : operationGenericTypeArguments[typeArgumentToUse.Index]; // so we take float
            })
            .Where(t => t is not null)
            .ToArray()!;

        // So now we replace U with float in the generic definition and get the correct handler.
        return genericHandlerImplementationType.MakeGenericType(typesToUseToCloseTheGeneric);
    }

    private static Type GetResultType(Type operationType)
    {
        // retrieves the specific (closed version of) IOperation<Something> that this operation implements.
        var operationInterface = operationType
            .GetInterfaces()
            .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IOperation<>));

        // That <something> is the result type. Potentially Empty.
        return operationInterface.GenericTypeArguments[0];
    }

    private static Type GetHandlerOperationTypeArgument(Type handlerType)
    {
        var interfaces = handlerType.GetInterfaces();

        // This is the "closed" IOperationHandler<TOperation<Parameters>, TResult<Parameters>>
        // where TResult<Parameters> should be the proper one for the TOperation<Parameters>.
        // We can be sure of this statement, because otherwise generic constraints would throw errors.
        // Let me trust those who can write code better than I
        var theInterface = interfaces
            .Single(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IOperationHandler<,>));

        var interfaceTypeArguments = theInterface.GenericTypeArguments;
        return interfaceTypeArguments[0];
    }
}
