using Cqrs.Handlers;
using Cqrs.Operations;

namespace Business.Commands.Test;

public class Number(int value)
{
    public int Value { get; set; } = value;
}

public sealed class IncrementNumberCommand(Number number) : ICommand
{
    public readonly Number Number = number;
}

internal sealed class IncrementNumberCommandHandler : ICommandHandler<IncrementNumberCommand>
{
    public ValueTask HandleAsync(IncrementNumberCommand command, CancellationToken cancellationToken)
    {
        command.Number.Value++;
        return ValueTask.CompletedTask;
    }
}