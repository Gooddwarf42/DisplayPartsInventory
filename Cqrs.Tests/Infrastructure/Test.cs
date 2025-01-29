using System;
using Microsoft.Extensions.DependencyInjection;

namespace Cqrs.Tests.Infrastructure;

public abstract class Test : IDisposable
{
    private readonly IServiceScope _scope;

    protected Test()
    {
        // Initialize ServiceCollection
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddServices();

        // create a service provider scope for this test class
        var rootServiceProvider = serviceCollection.BuildServiceProvider();
        _scope = rootServiceProvider.CreateScope();
    }

    protected IServiceProvider ServiceProvider => _scope.ServiceProvider;

    public void Dispose()
    {
        _scope.Dispose();
    }
}