namespace Cqrs.Commands;

public interface ICommand : ICommand<Empty> { }
public interface ICommand<TResult> { }