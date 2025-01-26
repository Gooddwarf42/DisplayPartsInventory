using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cqrs.Mediator;
using Cqrs.Operations;
using Cqrs.Tests.Infrastructure;
using Cqrs.Tests.SampleOperations.Commands;
using Cqrs.Tests.SampleOperations.Events;
using Cqrs.Tests.SampleOperations.Queries;
using JetBrains.Annotations;
using Xunit;

namespace Cqrs.Tests;

[TestSubject(typeof(CqrsContext))]
public class CqrsContextTest : Test
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

    // ReSharper disable once ClassNeverInstantiated.Local
    private class TestMediator : IMediator
    {
        public ValueTask<TResult> RunAsync<TResult>(ICommand<TResult> command, CancellationToken cancellationToken = default)
            => throw new System.NotImplementedException();

        public ValueTask<TResult> RunAsync<TResult>(IQuery<TResult> query, CancellationToken cancellationToken = default)
            => throw new System.NotImplementedException();

        public ValueTask RunAsync(IEvent @event, CancellationToken cancellationToken = default)
            => throw new System.NotImplementedException();
    }
}