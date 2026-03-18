using System.Threading;
using System.Threading.Tasks;
using WF.Cqrs.Handlers;
using WF.Cqrs.Operations;

namespace WF.Cqrs.Tests.SampleOperations.Commands;

internal class Number(int value)
{
    public int Value { get; set; } = value;
}

internal sealed class IncrementNumberCommand(Number number) : ICommand
{
    public readonly Number Number = number;
}

// Changing the logic of the executor may affect tests!
internal sealed class IncrementNumberCommandHandler : ICommandHandler<IncrementNumberCommand>
{
    public ValueTask HandleAsync(IncrementNumberCommand command, CancellationToken cancellationToken)
    {
        command.Number.Value++;
        return ValueTask.CompletedTask;
    }
}