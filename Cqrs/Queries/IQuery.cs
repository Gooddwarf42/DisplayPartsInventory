namespace Cqrs.Queries;

public interface IQuery : IQuery<Empty> { }
public interface IQuery<TResult> { }