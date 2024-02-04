namespace Cqrs.Events;

public interface IEvent : IEvent<Empty> { }
public interface IEvent<TResult> { } // I am not sure an event with result makes sense, but oh well.