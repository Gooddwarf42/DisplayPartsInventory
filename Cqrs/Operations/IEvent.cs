namespace Cqrs.Operations;

public interface IEvent<TResult> : IOperation { } // I am not sure an event with result makes sense, but oh well.

public interface IEvent : IEvent<Empty> { }