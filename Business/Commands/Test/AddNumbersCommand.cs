using Cqrs.Handlers;
using Cqrs.Operations;

namespace Business.Commands.Test;

public sealed class AddNumbersCommand(params int[] numbers) : ICommand<int>
{
    public readonly int[] Numbers = numbers;
}

internal sealed class AddNumbersCommandHandler : ICommandHandler<AddNumbersCommand, int>
{
    public ValueTask<int> HandleAsync(AddNumbersCommand command, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(command.Numbers is null or [] ? 0 : command.Numbers.Sum());
}