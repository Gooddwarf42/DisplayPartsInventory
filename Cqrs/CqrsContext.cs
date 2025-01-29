using System.Reflection;
using Cqrs.Decorator;
using Cqrs.Handlers;
using Cqrs.Mediator;
using Cqrs.Operations;
using Utils.Extensions;

namespace Cqrs;

// Notes to self: Public methods are called to perform configuration of the Cqrs pattern
// anything not strictly related to that is obviously either private or internal.
// The latter case is relevant for stuff needed to register types into dependency injection
public class CqrsContext
{
    private readonly List<Assembly> _assembliesToScan = [];
    private readonly List<Type> _handlerTypes = [];
    private readonly List<DecoratorInfo> _decoratorTypes = [];
    internal Type MediatorType = typeof(DefaultMediator);
    internal IEnumerable<Type> HandlerTypes => _handlerTypes.AsEnumerable();
    internal IEnumerable<DecoratorInfo> DecoratorInfos => _decoratorTypes.AsEnumerable();

    /// <summary>
    /// Configures the mediator used for cqrs. If this method is not invoked,
    /// the <see cref="DefaultMediator"/> is used instead
    /// </summary>
    public CqrsContext WithMediator<T>()
        where T : class, IMediator
    {
        MediatorType = typeof(T);
        return this;
    }

    public CqrsContext AddOperationHandler<TOperationHandler>()
        where TOperationHandler : class, IOperationHandler
    {
        var handlerTypeToAdd = typeof(TOperationHandler);
        if (handlerTypeToAdd is not { IsAbstract: false })
        {
            throw new ArgumentException($"Can't register {handlerTypeToAdd.Name} as an Operation Handler. It is an abstract class", nameof(TOperationHandler));
        }

        _handlerTypes.AddIfNotPresent(handlerTypeToAdd);
        return this;
    }

    public CqrsContext AddOperationHandler(Type operationHandlerType)
    {
        if (!operationHandlerType.Extends<IOperationHandler>())
        {
            throw new ArgumentException($"Can't register {operationHandlerType.Name} as an Operation Handler. Does it extend {nameof(IOperationHandler)}?", nameof(operationHandlerType));
        }

        if (operationHandlerType is not { IsAbstract: false, IsInterface: false })
        {
            throw new ArgumentException($"Can't register {operationHandlerType.Name} as an Operation Handler. It is an abstract class", nameof(operationHandlerType));
        }

        _handlerTypes.AddIfNotPresent(operationHandlerType);
        return this;
    }

    public CqrsContext AddAssembly(Type type)
    {
        var assembly = type.Assembly;
        _assembliesToScan.AddIfNotPresent(assembly);
        return this;
    }

    public CqrsContext AddAssembly(Assembly assembly)
    {
        _assembliesToScan.AddIfNotPresent(assembly);
        return this;
    }

    public CqrsContext AddDecorator(Type decoratorType, int order)
        => AddDecorator(decoratorType, order, _ => true);

    public CqrsContext AddDecorator(Type decoratorType, int order, Func<Type, bool> predicate)
    {
        if (!decoratorType.Extends<IDecorator>())
        {
            throw new ArgumentOutOfRangeException
            (
                nameof(decoratorType),
                $"Can't add decorator {decoratorType}.It does not extend {nameof(IDecorator)}"
            );
        }

        if (_decoratorTypes.Any(rule => rule.Order == order))
        {
            throw new ArgumentException($"Can't add decorator {decoratorType}. A decorator with the same order has already been added", nameof(order));
        }


        if (_decoratorTypes.Any(rule => rule.DecoratorType == decoratorType))
        {
            throw new ArgumentException($"Can't add decorator {decoratorType}. It has already been added", nameof(decoratorType));
        }

        _decoratorTypes.Add(new DecoratorInfo(decoratorType, order, predicate));

        return this;
    }

    internal void ScanAssemblies()
    {
        var handlerTypes = _assembliesToScan
            .SelectMany(a => a.DefinedTypes)
            .Where
            (
                type => type is { IsAbstract: false, IsInterface: false }
                        && type.Extends<IOperationHandler>()
                        && !type.Extends<IDecorator>()
            );

        _handlerTypes.AddWithoutDuplicates(handlerTypes);
    }

    internal IEnumerable<Type> GetDecoratorsTypes<TResult>(IOperation<TResult> operation)
        => GetDecoratorsTypes(operation.GetType());

    internal IEnumerable<Type> GetDecoratorsTypes(Type operationType)
        => _decoratorTypes
            .Where(t => t.Predicate(operationType))
            .OrderBy(t => t.Order)
            .Select(t => t.DecoratorType);

    internal record DecoratorInfo(Type DecoratorType, int Order, Func<Type, bool> Predicate);
}