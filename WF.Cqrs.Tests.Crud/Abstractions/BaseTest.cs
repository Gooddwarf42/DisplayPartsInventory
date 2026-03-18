using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WF.Cqrs.Crud.Extensions;
using WF.Cqrs.Decorator;
using WF.Cqrs.Extensions;
using WF.Cqrs.Mediator;
using WF.Cqrs.Tests.Crud.Entities;
using WF.Cqrs.Tests.Crud.ResolvedDependencies;
using WF.Data.Context;
using WF.Data.Extensions;
using WF.Mapper;
using WF.Mapper.Extensions;

namespace WF.Cqrs.Tests.Crud.Abstractions;

public abstract class BaseTest : IDisposable
{
    protected readonly IServiceScope Scope;
    protected readonly IMediator Mediator;
    protected readonly ApplicationDbContext DbContext;

    protected readonly User[] SeededUsers =
    [
        new() { Id = Guid.Parse("3279F69C-54B5-49B7-AD5E-F223F1572C66"), Age = 40, Name = "Gigi", Surname = "Pethot" },
        new() { Id = Guid.Parse("9AE353E9-8AC7-4454-8FB8-FE3D1CDE8D6E"), Age = 25, Name = "Harold", Surname = "Philips" },
    ];

    protected BaseTest()
    {
        // Initialize ServiceCollection
        var serviceCollection = new ServiceCollection();

        serviceCollection
            .AddData<DbContextConfigurator>()
            .AddMapper<DefaultMapper>(typeof(BaseTest)) // TODO wrap this into something maybe?
            .AddCqrs(context => context.AddCrud());

        // create a service provider scope for this test class
        var rootServiceProvider = serviceCollection.BuildServiceProvider();
        Scope = rootServiceProvider.CreateScope();
        var serviceProvider = Scope.ServiceProvider;
        Mediator = serviceProvider.GetRequiredService<IMediator>();
        DbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();

        SeedData();
        DbContext.ChangeTracker.Clear(); // Just to ensure no weird interactions happen
    }

    private void SeedData()
    {
        DbContext.Database.EnsureCreated();
        DbContext.AddRange(SeededUsers.Cast<object>()); // just to suppress the warning
        DbContext.SaveChanges();
    }

    public void Dispose()
    {
        Scope?.Dispose();
    }
}