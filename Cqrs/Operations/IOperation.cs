namespace Cqrs.Operations;

public interface IBaseOperation; //Just for interface marking
public interface IOperation<TResult> : IBaseOperation;
public interface IOperation : IOperation<Empty>;
