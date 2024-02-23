namespace Cqrs.Operations;

// ReSharper disable once UnusedTypeParameter
public interface IQuery<TResult> : IOperation;

public interface IQuery : IQuery<Empty>;