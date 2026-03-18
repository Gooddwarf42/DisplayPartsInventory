using System.Threading;
using System.Threading.Tasks;
using WF.Cqrs.Decorator;
using WF.Cqrs.Handlers;
using WF.Cqrs.Operations;

namespace WF.Cqrs.Tests.SampleDecorators;

internal class AppendADecorator<TOperation, TResult>(IOperationHandler<TOperation, TResult> decoratee, TestTracesService tracesService) : BaseDecorator<TOperation, TResult>(decoratee)
    where TOperation : IOperation<TResult>
{
    protected override ValueTask<TResult> DecorateAsync(IOperationHandler<TOperation, TResult> decoratee, TOperation operation, CancellationToken cancellationToken)
    {
        tracesService.TestCharacterList.Add('A');
        return decoratee.HandleAsync(operation, cancellationToken);
    }
}
