using System;
using System.Linq;
using Cqrs.Decorator;
using Cqrs.Operations;
using Cqrs.Tests.SampleOperations.Commands;
using Cqrs.Tests.SampleOperations.Events;
using Cqrs.Tests.SampleOperations.Queries;
using Xunit;

namespace Cqrs.Tests.Decorator;

public class DecorationFiltersTest

{
    [Theory]
    [InlineData(typeof(TestCommand))]
    [InlineData(typeof(AddNumbersCommand))]
    [InlineData(typeof(IncrementNumberCommand))]
    [InlineData(typeof(GetAnswerQuery))]
    [InlineData(typeof(SampleEvent))]
    public void All(Type commandType)
    {
        // Arrange
        var filter = DecorationFilters.All();

        // Act
        var result = filter(commandType);

        // Assert
        Assert.True(result);
    }

    [Theory]
    [InlineData(typeof(TestCommand))]
    [InlineData(typeof(AddNumbersCommand))]
    [InlineData(typeof(IncrementNumberCommand))]
    [InlineData(typeof(GetAnswerQuery))]
    [InlineData(typeof(SampleEvent))]
    public void OfType(Type commandType)
    {
        // Arrange
        var filter = DecorationFilters.OfType<AddNumbersCommand>();
        Type[] expectedTrue =
        [
            typeof(AddNumbersCommand),
        ];

        // Act and Assert
        TestFilter(commandType, filter, expectedTrue);
    }

    [Theory]
    [InlineData(typeof(TestCommand))]
    [InlineData(typeof(AddNumbersCommand))]
    [InlineData(typeof(IncrementNumberCommand))]
    [InlineData(typeof(GetAnswerQuery))]
    [InlineData(typeof(SampleEvent))]
    public void IsCommand(Type commandType)
    {
        // Arrange
        var filter = DecorationFilters.IsCommand();
        Type[] expectedTrue =
        [
            typeof(TestCommand),
            typeof(AddNumbersCommand),
            typeof(IncrementNumberCommand),
        ];

        // Act and Assert
        TestFilter(commandType, filter, expectedTrue);
    }

    [Theory]
    [InlineData(typeof(TestCommand))]
    [InlineData(typeof(AddNumbersCommand))]
    [InlineData(typeof(IncrementNumberCommand))]
    [InlineData(typeof(GetAnswerQuery))]
    [InlineData(typeof(SampleEvent))]
    public void IsQuery(Type commandType)
    {
        // Arrange
        var filter = DecorationFilters.IsQuery();
        Type[] expectedTrue =
        [
            typeof(GetAnswerQuery),
        ];

        // Act and Assert
        TestFilter(commandType, filter, expectedTrue);
    }

    [Theory]
    [InlineData(typeof(TestCommand))]
    [InlineData(typeof(AddNumbersCommand))]
    [InlineData(typeof(IncrementNumberCommand))]
    [InlineData(typeof(GetAnswerQuery))]
    [InlineData(typeof(SampleEvent))]
    public void IsEvent(Type commandType)
    {
        // Arrange
        var filter = DecorationFilters.IsEvent();
        Type[] expectedTrue =
        [
            typeof(SampleEvent),
        ];

        // Act and Assert
        TestFilter(commandType, filter, expectedTrue);
    }

    [Theory]
    [InlineData(typeof(TestCommand))]
    [InlineData(typeof(AddNumbersCommand))]
    [InlineData(typeof(IncrementNumberCommand))]
    [InlineData(typeof(GetAnswerQuery))]
    [InlineData(typeof(SampleEvent))]
    public void HasAttribute(Type commandType)
    {
        // Arrange
        var filter = DecorationFilters.HasAttribute<TestAttribute>();
        Type[] expectedTrue =
        [
            typeof(TestCommand),
        ];

        // Act and Assert
        TestFilter(commandType, filter, expectedTrue);
    }

    private void TestFilter(Type commandType, Func<Type, bool> filter, Type[] expectedTrue)
    {
        // Act
        var result = filter(commandType);

        // Assert
        if (expectedTrue.Contains(commandType))
        {
            Assert.True(result);
        }
        else
        {
            Assert.False(result);
        }
    }

    [AttributeUsage(AttributeTargets.Class)]
    private class TestAttribute : Attribute;

    [Test]
    private class TestCommand : ICommand;
}