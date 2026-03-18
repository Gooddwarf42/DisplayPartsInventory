using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using WF.Cqrs.Decorator;
using WF.Cqrs.Extensions;
using WF.Cqrs.Mediator;
using WF.Cqrs.Tests.SampleDecorators;
using WF.Cqrs.Tests.SampleOperations.Commands;
using WF.Cqrs.Tests.SampleOperations.Events;
using WF.Cqrs.Tests.SampleOperations.Queries;
using WF.Cqrs.Tests.Services;
using Xunit;

namespace WF.Cqrs.Tests.Mediator;

[TestSubject(typeof(DefaultMediator))]
public class DefaultMediatorTest : IDisposable
{
    private readonly DefaultMediator _mediator;
    private readonly IServiceScope _scope;
    private readonly TestTracesService _tracesService;

    public DefaultMediatorTest()
    {
        // Initialize ServiceCollection
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddScoped<TestTracesService>();

        serviceCollection.AddCqrs
        (context => context
            .AddAssembly(typeof(DefaultMediatorTest))
            .AddDecorator(typeof(AppendADecorator<,>), 0)
            .AddDecorator(typeof(AppendBDecorator<,>), 1, DecorationFilters.IsCommand())
        );

        // create a service provider scope for this test class
        var rootServiceProvider = serviceCollection.BuildServiceProvider();
        _scope = rootServiceProvider.CreateScope();
        var serviceProvider = _scope.ServiceProvider;
        _mediator = (DefaultMediator) serviceProvider.GetRequiredService<IMediator>();
        _tracesService = serviceProvider.GetRequiredService<TestTracesService>();
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

        var sampleEvent = new SampleEvent(() =>
        {
            testNumber++;
        });

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
        Assert.Equal(expectedTestString, string.Join("", _tracesService.TestCharacterList));
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
        Assert.Equal(expectedTestString, string.Join("", _tracesService.TestCharacterList));
    }

    [Theory]
    [InlineData(typeof(AddNumbersCommand), typeof(int), typeof(AddNumbersCommandHandler))]
    [InlineData(typeof(IncrementNumberCommand), typeof(Empty), typeof(IncrementNumberCommandHandler))]
    [InlineData(typeof(GetAnswerQuery), typeof(int), typeof(GetAnswerQueryHandler))]
    [InlineData(typeof(SampleEvent), typeof(Empty), typeof(SampleEventHandler))]
    [InlineData(typeof(GenericWithFixedReturnTypeCommand<float>), typeof(int), typeof(GenericWithFixedReturnTypeCommandHandler<float>))]
    [InlineData(typeof(GenericWithTypeParameterAsReturnTypeCommand<float>), typeof(float), typeof(GenericWithTypeParameterAsReturnTypeCommandHandler<float>))]
    [InlineData(typeof(GenericWithClosedGenericAsReturnTypeCommand<float>), typeof(List<int>), typeof(GenericWithClosedGenericAsReturnTypeCommandHandler<float>))]
    [InlineData(typeof(GenericWithClosedGenericUsingTypeParametersAsReturnTypeCommand<float>), typeof(List<float>), typeof(GenericWithClosedGenericUsingTypeParametersAsReturnTypeCommandHandler<float>))]
    [InlineData(typeof(HarderCaseCommand<float, char>), typeof(List<char>), typeof(HarderCaseCommandHandler<char, float>))]
    public void ResolvesCorrectHandler(Type operationType, Type resultType, Type expectedHandlerType)
    {
        // This is not great, I really should refactor the method itself...
        // result type should be extracted from the operation type...
        var actualHandlerType = _mediator.GetHandlerImplementationType(operationType, resultType);
        Assert.Equal(expectedHandlerType, actualHandlerType);
    }
}
