using System.Reflection;
using Cqrs.Handlers;
using Cqrs.Mediator;
using Utils.Extensions;

namespace Cqrs;

// Notes to self: Public methods are called to perform configuration of the Cqrs pattern
// anything not strictly related to that is obviously either private or internal.
// The latter case is relevant for stuff needed to register types into dependency injection
public class CqrsContext
{
    private readonly List<Assembly> _assembliesToScan = [];
    private readonly List<Type> _handlerTypes = [];
    internal Type MediatorType = typeof(DefaultMediator);
    internal IEnumerable<Type> HandlerTypes => _handlerTypes.AsEnumerable();

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

    internal void ScanAssemblies()
    {
        var handlerTypes = _assembliesToScan
            .SelectMany(a => a.DefinedTypes)
            .Where
            (
                type => type is { IsAbstract: false, IsInterface: false }
                        && type.Extends<IOperationHandler>()
            );

        _handlerTypes.AddWithoutDuplicates(handlerTypes);
    }
}