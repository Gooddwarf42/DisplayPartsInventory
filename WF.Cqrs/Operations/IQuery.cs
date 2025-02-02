namespace WF.Cqrs.Operations;

public interface IBaseQuery : IBaseOperation; //Just for interface marking

// ReSharper disable once UnusedTypeParameter
public interface IQuery<TResult> : IOperation<TResult>, IBaseQuery;