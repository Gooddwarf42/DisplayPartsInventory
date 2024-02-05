namespace Cqrs.Operations;

public interface ICommand<TResult> : IOperation { }

public interface ICommand : ICommand<Empty> { }