using System.Reflection;
using Cqrs.Handlers;
using Cqrs.Mediator;
using Utils.Extensions;

namespace Cqrs;

// Notes to self: Public methods are called to perform configuration of the Cqrs pattern
// anything not strictly related to that is obviously either private or internal.
// The latter case is relevant for stuff needed to register types into dependency injection
public class CqrsConfiguration
{
    private readonly List<Assembly> _assembliesToScan = [];
    private readonly List<Type> _handlerTypes = [];
    internal Type MediatorType = typeof(DefaultMediator);
    internal IEnumerable<Type> HandlerTypes => _handlerTypes.AsEnumerable();

    /// <summary>
    /// Configures the mediator used for cqrs. If this method is not invoked,
    /// the <see cref="DefaultMediator"/> is used instead
    /// </summary>
    public CqrsConfiguration WithMediator(Type mediatorType)
    {
        MediatorType = mediatorType;
        return this;
    }

    public CqrsConfiguration AddOperationHandler<TOperationHandler>()
        where TOperationHandler : IOperationHandler
    {
        _handlerTypes.AddIfNotPresent(typeof(TOperationHandler));
        return this;
    }

    public CqrsConfiguration AddOperationHandler(Type operationHandlerType)
    {
        if (!operationHandlerType.Extends<IOperationHandler>())
        {
            throw new ArgumentException($"Can't register {operationHandlerType.Name} as an Operation Handler. Does it extedn {nameof(IOperationHandler)}?", nameof(operationHandlerType));
        }

        _handlerTypes.AddIfNotPresent(operationHandlerType);
        return this;
    }

    public CqrsConfiguration AddAssembly(Assembly assembly)
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
                type => type is { IsAbstract: false, IsInterface: false, }
                        && type.Extends<IOperationHandler>()
            );

        _handlerTypes.AddWithoutDuplicates(handlerTypes);
    }
}