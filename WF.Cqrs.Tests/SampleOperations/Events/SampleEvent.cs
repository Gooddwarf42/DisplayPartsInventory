using System;
using System.Threading;
using System.Threading.Tasks;
using WF.Cqrs.Handlers;
using WF.Cqrs.Operations;

namespace WF.Cqrs.Tests.SampleOperations.Events;

internal sealed class SampleEvent(Action action) : IEvent
{
    public readonly Action Action = action;
}

// Changing the logic of the executor may affect tests!
internal sealed class SampleEventHandler : IEventHandler<SampleEvent>
{
    public ValueTask HandleAsync(SampleEvent @event, CancellationToken cancellationToken = default)
    {
        Console.WriteLine("Executing sampleEvent:");
        @event.Action();
        return ValueTask.CompletedTask;
    }
}