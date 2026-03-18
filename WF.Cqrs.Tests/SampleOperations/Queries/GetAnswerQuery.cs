using System.Threading;
using System.Threading.Tasks;
using WF.Cqrs.Handlers;
using WF.Cqrs.Operations;

namespace WF.Cqrs.Tests.SampleOperations.Queries;

internal sealed class GetAnswerQuery : IQuery<int>;

// Changing the logic of the executor may affect tests!
internal sealed class GetAnswerQueryHandler : IQueryHandler<GetAnswerQuery, int>
{
    public ValueTask<int> HandleAsync(GetAnswerQuery query, CancellationToken cancellationToken = default)
        => ValueTask.FromResult(42);
}