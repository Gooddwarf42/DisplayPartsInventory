using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WF.Cqrs.Handlers;
using WF.Cqrs.Operations;

namespace WF.Cqrs.Tests.SampleOperations.Commands;

internal sealed class AddNumbersCommand(params int[] numbers) : ICommand<int>
{
    public readonly int[] Numbers = numbers;
}

// Changing the logic of the executor may affect tests!
internal sealed class AddNumbersCommandHandler : ICommandHandler<AddNumbersCommand, int>
{
    public ValueTask<int> HandleAsync(AddNumbersCommand command, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(command.Numbers is null or [] ? 0 : command.Numbers.Sum());
}