using System;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using WF.Cqrs.Extensions;
using WF.Cqrs.Tests.Mediator;
using WF.Cqrs.Tests.SampleOperations.Commands;
using WF.Cqrs.Tests.SampleOperations.Events;
using WF.Cqrs.Tests.SampleOperations.Queries;
using Xunit;

namespace WF.Cqrs.Tests.Extensions;

[TestSubject(typeof(TypeExtensions))]
public class TypeExtensionsTest : IDisposable
{
    private readonly IServiceScope _scope;
    private readonly CqrsContext _cqrsContext;

    public TypeExtensionsTest()
    {
        // Initialize ServiceCollection
        var serviceCollection = new ServiceCollection();

        serviceCollection.AddScoped<TestTracesService>();

        serviceCollection.AddCqrs
        (
            context => context
                .AddAssembly(typeof(DefaultMediatorTest))
        );

        // create a service provider scope for this test class
        var rootServiceProvider = serviceCollection.BuildServiceProvider();
        _scope = rootServiceProvider.CreateScope();
        var serviceProvider = _scope.ServiceProvider;
        serviceProvider.GetRequiredService<TestTracesService>();
        _cqrsContext = serviceProvider.GetRequiredService<CqrsContext>();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }

    [Theory]
    [InlineData(typeof(AddNumbersCommand), typeof(AddNumbersCommandHandler))]
    [InlineData(typeof(IncrementNumberCommand), typeof(IncrementNumberCommandHandler))]
    [InlineData(typeof(GetAnswerQuery), typeof(GetAnswerQueryHandler))]
    [InlineData(typeof(SampleEvent), typeof(SampleEventHandler))]
    [InlineData(typeof(GenericWithFixedReturnTypeCommand<float>), typeof(GenericWithFixedReturnTypeCommandHandler<float>))]
    [InlineData(typeof(GenericWithTypeParameterAsReturnTypeCommand<float>), typeof(GenericWithTypeParameterAsReturnTypeCommandHandler<float>))]
    [InlineData(typeof(GenericWithClosedGenericAsReturnTypeCommand<float>), typeof(GenericWithClosedGenericAsReturnTypeCommandHandler<float>))]
    [InlineData(typeof(GenericWithClosedGenericUsingTypeParametersAsReturnTypeCommand<float>), typeof(GenericWithClosedGenericUsingTypeParametersAsReturnTypeCommandHandler<float>))]
    [InlineData(typeof(HarderCaseCommand<float, char>), typeof(HarderCaseCommandHandler<char, float>))]
    [InlineData(typeof(MyCommand<char, float>), typeof(MyCommandHandler<float>))]
    public void ResolvesCorrectHandler(Type operationType, Type expectedHandlerType)
    {
        var actualHandlerType = operationType.GetOperationHandlerImplementationType(_cqrsContext.HandlerTypes);
        Assert.Equal(expectedHandlerType, actualHandlerType);
    }
}