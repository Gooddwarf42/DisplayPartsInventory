using Cqrs.Operations;

namespace Cqrs.Handlers;

// TODO: this is still very much a draft!
public interface IOperationHandler;

public interface IOperationHandler<in TOperation> : IOperationHandler
    where TOperation: IOperation;