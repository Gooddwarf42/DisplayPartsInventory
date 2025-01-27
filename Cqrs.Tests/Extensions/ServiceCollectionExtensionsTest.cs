using System;
using System.Linq;
using Cqrs.Extensions;
using Cqrs.Mediator;
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
                .AddAssembly(typeof(ServiceCollectionExtensionsTest));

        const int expectedHandlers = 4;
        var expectedMediatorType = typeof(DefaultMediator);

        // Act
        serviceCollection.AddCqrs(configuration);
        var rootServiceProvider = serviceCollection.BuildServiceProvider();

        // Assert

        var cqrsContext = rootServiceProvider.GetRequiredService<CqrsContext>();

        Assert.Equal(expectedHandlers, cqrsContext.HandlerTypes.Count());
        Assert.Equal(expectedMediatorType, cqrsContext.MediatorType);
    }
}