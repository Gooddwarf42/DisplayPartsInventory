using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cqrs.Decorator;
using Cqrs.Extensions;
using Cqrs.Handlers;
using Cqrs.Mediator;
using Cqrs.Operations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cqrs.Tests.Extensions;

public class ServiceCollectionExtensionsTest
{
    [Fact]
    public void AddCqrs()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        // ReSharper disable once ConvertToLocalFunction
        Action<CqrsContext> configuration = cqrsContext
            => cqrsContext
                .AddAssembly(typeof(ServiceCollectionExtensionsTest))
                .AddDecorator(typeof(TestDecorator<,>), 0);

        const int expectedHandlers = 4;
        const int expectedDecorators = 1;
        var expectedMediatorType = typeof(DefaultMediator);

        // Act
        serviceCollection.AddCqrs(configuration);
        var rootServiceProvider = serviceCollection.BuildServiceProvider();

        // Assert
        var cqrsContext = rootServiceProvider.GetRequiredService<CqrsContext>();

        Assert.Equal(expectedHandlers, cqrsContext.HandlerTypes.Count());
#pragma warning disable xUnit2013
        Assert.Equal(expectedDecorators, cqrsContext.DecoratorInfos.Count());
#pragma warning restore xUnit2013
        Assert.Equal(expectedMediatorType, cqrsContext.MediatorType);
    }

    private class TestDecorator<TOperation, TResult>(IOperationHandler<TOperation, TResult> decoratee) : BaseDecorator<TOperation, TResult>(decoratee) where TOperation : IOperation<TResult>
    {
        protected override ValueTask<TResult> DecorateAsync(IOperationHandler<TOperation, TResult> decoratee, TOperation operation, CancellationToken cancellationToken)
            => throw new NotImplementedException();
    }
}