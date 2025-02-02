namespace WF.Cqrs.Operations;

public interface IBaseCommand : IBaseOperation; //Just for interface marking

// ReSharper disable once UnusedTypeParameter
public interface ICommand<TResult> : IOperation<TResult>, IBaseCommand;

public interface ICommand : ICommand<Empty>, IOperation;