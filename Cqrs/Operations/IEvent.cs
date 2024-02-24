namespace Cqrs.Operations;

public interface IBaseEvent : IBaseOperation; //Just for interface marking

public interface IEvent : IOperation, IBaseEvent;