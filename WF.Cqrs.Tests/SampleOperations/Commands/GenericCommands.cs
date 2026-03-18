using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using WF.Cqrs.Handlers;
using WF.Cqrs.Operations;

namespace WF.Cqrs.Tests.SampleOperations.Commands;

#region FIXED RETURN TYPE

internal sealed class GenericCommandWithFixedReturnTypeCommand<T> : ICommand<int>;

internal sealed class GenericCommandWithFixedReturnTypeCommandHandler<T> : IOperationHandler<GenericCommandWithFixedReturnTypeCommand<T>, int>
{
    public ValueTask<int> HandleAsync(GenericCommandWithFixedReturnTypeCommand<T> operation, CancellationToken cancellationToken = default)
        => throw new System.NotImplementedException();
}

#endregion

#region TYPE PARAMETER AS RETURN TYPE

internal sealed class GenericCommandWithTypeParameterAsReturnTypeCommand<T> : ICommand<T>;

internal sealed class GenericCommandWithTypeParameterAsReturnTypeCommandHandler<T> : IOperationHandler<GenericCommandWithTypeParameterAsReturnTypeCommand<T>, T>
{
    public ValueTask<T> HandleAsync(GenericCommandWithTypeParameterAsReturnTypeCommand<T> operation, CancellationToken cancellationToken = default)
        => throw new System.NotImplementedException();
}

#endregion

#region CLOSED GENERIC AS RETURN TYPE

internal sealed class GenericCommandWithClosedGenericAsReturnTypeCommand<T> : ICommand<List<int>>;

internal sealed class GenericCommandWithClosedGenericAsReturnTypeCommandHandler<T> : IOperationHandler<GenericCommandWithClosedGenericAsReturnTypeCommand<T>, List<int>>
{
    public ValueTask<List<int>> HandleAsync(GenericCommandWithClosedGenericAsReturnTypeCommand<T> operation, CancellationToken cancellationToken = default)
        => throw new System.NotImplementedException();
}

#endregion

#region CLOSED GENERIC WITH TYPE ARGUMENT AS RETURN TYPE

internal sealed class GenericCommandWithClosedGenericUsingTypeParametersAsReturnTypeCommand<T> : ICommand<List<T>>;

internal sealed class GenericCommandWithClosedGenericUsingTypeParametersAsReturnTypeCommandHandler<T> : IOperationHandler<GenericCommandWithClosedGenericUsingTypeParametersAsReturnTypeCommand<T>, List<T>>
{
    public ValueTask<List<T>> HandleAsync(GenericCommandWithClosedGenericUsingTypeParametersAsReturnTypeCommand<T> operation, CancellationToken cancellationToken = default)
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
