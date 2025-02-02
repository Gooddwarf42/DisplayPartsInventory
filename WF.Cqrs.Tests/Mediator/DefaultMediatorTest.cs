using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using WF.Cqrs.Decorator;
using WF.Cqrs.Extensions;
using WF.Cqrs.Handlers;
using WF.Cqrs.Mediator;
using WF.Cqrs.Operations;
using WF.Cqrs.Tests.SampleOperations.Commands;
using WF.Cqrs.Tests.SampleOperations.Events;
using WF.Cqrs.Tests.SampleOperations.Queries;
using Xunit;

namespace WF.Cqrs.Tests.Mediator;

[TestSubject(typeof(DefaultMediator))]
public class DefaultMediatorTest : IDisposable
{
    private static readonly List<char> TestCharacterList = [];
    private readonly DefaultMediator _mediator;
    private readonly IServiceScope _scope;

    public DefaultMediatorTest()
    {
        // Initialize ServiceCollection
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddCqrs
        (
            context => context
                .AddAssembly(typeof(DefaultMediatorTest))
                .AddDecorator(typeof(AppendADecorator<,>), 0)
                .AddDecorator(typeof(AppendBDecorator<,>), 1, DecorationFilters.IsCommand())
        );

        // create a service provider scope for this test class
        var rootServiceProvider = serviceCollection.BuildServiceProvider();
        _scope = rootServiceProvider.CreateScope();
        var serviceProvider = _scope.ServiceProvider;
        _mediator = (DefaultMediator)serviceProvider.GetRequiredService<IMediator>();
    }


    public void Dispose()
    {
        _scope.Dispose();
    }

    // Note to self: separate tests run on separate instances of the class
    [Fact]
    public async Task CommandWithReturnTypeExecutes()
    {
        // Arrange
        int[] numbers = [2, 3];
        var command = new AddNumbersCommand(numbers);
        const int expectedResult = 5;

        // Act
        var result = await _mediator.RunAsync(command);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task CommandWithoutReturnTypeExecutes()
    {
        // Arrange
        var number = new Number(42);
        var command = new IncrementNumberCommand(number);
        const int expectedResult = 43;

        // Act
        await _mediator.RunAsync(command);

        // Assert
        Assert.Equal(expectedResult, number.Value);
    }

    [Fact]
    public async Task QueryExecutes()
    {
        // Arrange
        var query = new GetAnswerQuery();
        const int expectedResult = 42;

        // Act
        var result = await _mediator.RunAsync(query);

        // Assert
        Assert.Equal(expectedResult, result);
    }

    [Fact]
    public async Task EventExecutes()
    {
        // Arrange
        var testNumber = 4;
        var sampleEvent = new SampleEvent(() => { testNumber++; });
        const int expectedResult = 5;

        // Act
        await _mediator.RunAsync(sampleEvent);

        // Assert
        Assert.Equal(expectedResult, testNumber);
    }

    [Fact]
    public async Task AppliesDecoratorsInOrder()
    {
        // Arrange
        var number = new Number(42);
        var command = new IncrementNumberCommand(number);
        const string expectedTestString = "AB";

        // Act
        await _mediator.RunAsync(command);

        // Assert
        Assert.Equal(expectedTestString, string.Join("", TestCharacterList));
    }

    [Fact]
    public async Task AppliesDecoratorsOnlyWhenNeeded()
    {
        // Arrange
        var query = new GetAnswerQuery();
        const string expectedTestString = "A";

        // Act
        await _mediator.RunAsync(query);

        // Assert
        Assert.Equal(expectedTestString, string.Join("", TestCharacterList));
    }

    private class AppendADecorator<TOperation, TResult>(IOperationHandler<TOperation, TResult> decoratee) : BaseDecorator<TOperation, TResult>(decoratee) where TOperation : IOperation<TResult>
    {
        protected override ValueTask<TResult> DecorateAsync(IOperationHandler<TOperation, TResult> decoratee, TOperation operation, CancellationToken cancellationToken)
        {
            TestCharacterList.Add('A');
            return decoratee.HandleAsync(operation, cancellationToken);
        }
    }

    private class AppendBDecorator<TOperation, TResult>(IOperationHandler<TOperation, TResult> decoratee) : BaseDecorator<TOperation, TResult>(decoratee) where TOperation : IOperation<TResult>
    {
        protected override ValueTask<TResult> DecorateAsync(IOperationHandler<TOperation, TResult> decoratee, TOperation operation, CancellationToken cancellationToken)
        {
            TestCharacterList.Add('B');
            return decoratee.HandleAsync(operation, cancellationToken);
        }
    }
}