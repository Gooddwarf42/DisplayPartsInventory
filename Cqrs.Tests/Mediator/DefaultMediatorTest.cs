using System.Threading.Tasks;
using Cqrs.Mediator;
using Cqrs.Tests.Infrastructure;
using Cqrs.Tests.SampleOperations.Commands;
using Cqrs.Tests.SampleOperations.Events;
using Cqrs.Tests.SampleOperations.Queries;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Cqrs.Tests.Mediator;

[TestSubject(typeof(DefaultMediator))]
public class DefaultMediatorTest : Test
{
    private readonly DefaultMediator _mediator;

    public DefaultMediatorTest()
    {
        _mediator = (DefaultMediator)ServiceProvider.GetRequiredService<IMediator>();
    }

    // TODO check whether facts are ran on separate intances of the class! I don't remember this detail yet, and I am lazy.
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
    public async Task CommandWithOutReturnTypeExecutes()
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
}