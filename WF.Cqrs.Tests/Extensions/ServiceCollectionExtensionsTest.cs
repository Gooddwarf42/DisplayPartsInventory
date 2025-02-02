using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using WF.Cqrs.Decorator;
using WF.Cqrs.Extensions;
using WF.Cqrs.Handlers;
using WF.Cqrs.Mediator;
using WF.Cqrs.Operations;
using Xunit;

namespace WF.Cqrs.Tests.Extensions;

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

    [Fact]
    public void Should_Throw_When_AddingSameDecorator()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        // ReSharper disable once ConvertToLocalFunction
        Action<CqrsContext> configuration = cqrsContext
            => cqrsContext
                .AddAssembly(typeof(ServiceCollectionExtensionsTest))
                .AddDecorator(typeof(TestDecorator<,>), 0)
                .AddDecorator(typeof(TestDecorator<,>), 1);

        // Act  // Assert
        Assert.Throws<ArgumentException>(() => serviceCollection.AddCqrs(configuration));
    }

    [Fact]
    public void Should_Throw_When_AddingDecoratorsWithSameOrder()
    {
        // Arrange
        var serviceCollection = new ServiceCollection();
        // ReSharper disable once ConvertToLocalFunction
        Action<CqrsContext> configuration = cqrsContext
            => cqrsContext
                .AddAssembly(typeof(ServiceCollectionExtensionsTest))
                .AddDecorator(typeof(TestDecorator<,>), 0)
                .AddDecorator(typeof(TestDecorator2<,>), 0);

        // Act  // Assert
        Assert.Throws<ArgumentException>(() => serviceCollection.AddCqrs(configuration));
    }

    private class TestDecorator<TOperation, TResult>(IOperationHandler<TOperation, TResult> decoratee) : BaseDecorator<TOperation, TResult>(decoratee) where TOperation : IOperation<TResult>
    {
        protected override ValueTask<TResult> DecorateAsync(IOperationHandler<TOperation, TResult> decoratee, TOperation operation, CancellationToken cancellationToken)
            => throw new NotImplementedException();
    }

    private class TestDecorator2<TOperation, TResult>(IOperationHandler<TOperation, TResult> decoratee) : BaseDecorator<TOperation, TResult>(decoratee) where TOperation : IOperation<TResult>
    {
        protected override ValueTask<TResult> DecorateAsync(IOperationHandler<TOperation, TResult> decoratee, TOperation operation, CancellationToken cancellationToken)
            => throw new NotImplementedException();
    }
}