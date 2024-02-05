namespace Cqrs.Operations;

public interface IQuery<TResult> : IOperation { }
public interface IQuery : IQuery<Empty> { }