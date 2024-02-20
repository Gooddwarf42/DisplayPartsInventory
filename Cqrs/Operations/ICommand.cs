namespace Cqrs.Operations;

// ReSharper disable once UnusedTypeParameter
public interface ICommand<TResult> : IOperation;

public interface ICommand : ICommand<Empty>;