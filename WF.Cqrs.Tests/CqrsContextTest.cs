using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using WF.Cqrs.Decorator;
using WF.Cqrs.Handlers;
using WF.Cqrs.Mediator;
using WF.Cqrs.Operations;
using WF.Cqrs.Tests.SampleOperations.Commands;
using WF.Cqrs.Tests.SampleOperations.Events;
using WF.Cqrs.Tests.SampleOperations.Queries;
using Xunit;

namespace WF.Cqrs.Tests;

[TestSubject(typeof(CqrsContext))]
public class CqrsContextTest
{
    [Fact]
    public void HasDefaultMediator()
    {
        // Arrange
        var cqrsContext = new CqrsContext();

        // Act


        // Assert
        Assert.Equal(typeof(DefaultMediator), cqrsContext.MediatorType);
    }


    [Fact]
    public void AddMediator()
    {
        // Arrange
        var cqrsContext = new CqrsContext();

        // Act
        cqrsContext.WithMediator<TestMediator>();

        // Assert
        Assert.Equal(typeof(TestMediator), cqrsContext.MediatorType);
    }

    [Fact]
    public void AddHandlersFromAssembly()
    {
        // Arrange
        var cqrsContext = new CqrsContext();

        const int expectedHandlerCount = 4;
        List<Type> expectedHandlerTypes =
        [
            typeof(AddNumbersCommandHandler),
            typeof(IncrementNumberCommandHandler),
            typeof(GetAnswerQueryHandler),
            typeof(SampleEventHandler),
        ];
        SortHandlers(expectedHandlerTypes);

        // Act
        cqrsContext.AddAssembly(typeof(CqrsContextTest));
        cqrsContext.ScanAssemblies();

        // Assert
        var handlerTypesInCqrsContext = cqrsContext.HandlerTypes.ToList();
        SortHandlers(handlerTypesInCqrsContext);

        Assert.Equal(expectedHandlerCount, handlerTypesInCqrsContext.Count);
        for (var i = 0; i < expectedHandlerTypes.Count; i++)
        {
            Assert.Equal(expectedHandlerTypes[i], handlerTypesInCqrsContext[i]);
        }

        return;

        void SortHandlers(List<Type> handlersList)
        {
            handlersList.Sort((t1, t2) => string.Compare(t1.Name, t2.Name, StringComparison.Ordinal));
        }
    }

    [Fact]
    public void AddDecorator()
    {
        // Arrange
        var cqrsContext = new CqrsContext();

        const int expectedDecoratorCount = 2;
        List<Type> expectedDecoratorTypes =
        [
            typeof(TestDecorator<,>),
            typeof(TestDecorator2<,>),
        ];

        // Act
        cqrsContext.AddDecorator(typeof(TestDecorator<,>), 0);
        cqrsContext.AddDecorator(typeof(TestDecorator2<,>), 1, DecorationFilters.OfType<AddNumbersCommand>());

        // Assert
        var decoratorsInCqrsContext = cqrsContext.DecoratorInfos.ToList();

        Assert.Equal(expectedDecoratorCount, decoratorsInCqrsContext.Count);
        for (var i = 0; i < expectedDecoratorTypes.Count; i++)
        {
            Assert.Equal(expectedDecoratorTypes[i], decoratorsInCqrsContext[i].DecoratorType);
        }

        // Check if predicates have been added properly
        Assert.True(decoratorsInCqrsContext[0].Predicate(typeof(AddNumbersCommand)));
        Assert.True(decoratorsInCqrsContext[1].Predicate(typeof(AddNumbersCommand)));
        Assert.True(decoratorsInCqrsContext[0].Predicate(typeof(IncrementNumberCommand)));
        Assert.False(decoratorsInCqrsContext[1].Predicate(typeof(IncrementNumberCommand)));
    }


    // ReSharper disable once ClassNeverInstantiated.Local
    private class TestMediator : IMediator
    {
        public ValueTask<TResult> RunAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public ValueTask<TResult> RunAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();

        public ValueTask RunAsync(IEvent @event, CancellationToken cancellationToken = default)
            => throw new NotImplementedException();
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