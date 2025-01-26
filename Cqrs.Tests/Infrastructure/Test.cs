using System;
using Cqrs.Tests.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Cqrs.Tests.Infrastructure;

public abstract class Test : IDisposable
{
    private readonly IServiceScope _scope;
    protected IServiceProvider ServiceProvider => _scope.ServiceProvider;

    protected Test()
    {
        // Initialize ServiceCollection
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddServices();

        // create a service provider scope for this test class
        var rootServiceProvider = serviceCollection.BuildServiceProvider();
        _scope = rootServiceProvider.CreateScope();
    }

    public void Dispose()
    {
        _scope.Dispose();
    }
}