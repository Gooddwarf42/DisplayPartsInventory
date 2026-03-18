using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WF.Cqrs.Handlers;
using WF.Cqrs.Operations;

namespace WF.Cqrs.Tests.SampleOperations.Commands;

#region FIXED RETURN TYPE

internal sealed class GenericWithFixedReturnTypeCommand<T> : ICommand<int>;

internal sealed class GenericWithFixedReturnTypeCommandHandler<T> : IOperationHandler<GenericWithFixedReturnTypeCommand<T>, int>
{
    public ValueTask<int> HandleAsync(GenericWithFixedReturnTypeCommand<T> operation, CancellationToken cancellationToken = default)
        => throw new System.NotImplementedException();
}

#endregion

#region TYPE PARAMETER AS RETURN TYPE

internal sealed class GenericWithTypeParameterAsReturnTypeCommand<T> : ICommand<T>;

internal sealed class GenericWithTypeParameterAsReturnTypeCommandHandler<T> : IOperationHandler<GenericWithTypeParameterAsReturnTypeCommand<T>, T>
{
    public ValueTask<T> HandleAsync(GenericWithTypeParameterAsReturnTypeCommand<T> operation, CancellationToken cancellationToken = default)
        => throw new System.NotImplementedException();
}

#endregion

#region CLOSED GENERIC AS RETURN TYPE

internal sealed class GenericWithClosedGenericAsReturnTypeCommand<T> : ICommand<List<int>>;

internal sealed class GenericWithClosedGenericAsReturnTypeCommandHandler<T> : IOperationHandler<GenericWithClosedGenericAsReturnTypeCommand<T>, List<int>>
{
    public ValueTask<List<int>> HandleAsync(GenericWithClosedGenericAsReturnTypeCommand<T> operation, CancellationToken cancellationToken = default)
        => throw new System.NotImplementedException();
}

#endregion

#region CLOSED GENERIC WITH TYPE ARGUMENT AS RETURN TYPE

internal sealed class GenericWithClosedGenericUsingTypeParametersAsReturnTypeCommand<T> : ICommand<List<T>>;

internal sealed class GenericWithClosedGenericUsingTypeParametersAsReturnTypeCommandHandler<T> : IOperationHandler<GenericWithClosedGenericUsingTypeParametersAsReturnTypeCommand<T>, List<T>>
{
    public ValueTask<List<T>> HandleAsync(GenericWithClosedGenericUsingTypeParametersAsReturnTypeCommand<T> operation, CancellationToken cancellationToken = default)
        => throw new System.NotImplementedException();
}

#endregion

#region A HARDER CASE WITH MULTIPLE GENERIC

internal sealed class HarderCaseCommand<T, S> : ICommand<List<S>>;

internal sealed class HarderCaseCommandHandler<S, T> : IOperationHandler<HarderCaseCommand<T, S>, List<S>>
{
    public ValueTask<List<S>> HandleAsync(HarderCaseCommand<T, S> operation, CancellationToken cancellationToken = default)
        => throw new System.NotImplementedException();
}

#endregion
