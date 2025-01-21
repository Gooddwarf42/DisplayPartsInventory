namespace Cqrs.Operations;

public interface IBaseOperation; //Just for interface marking

// ReSharper disable once UnusedTypeParameter
public interface IOperation<TResult> : IBaseOperation;

public interface IOperation : IOperation<Empty>;