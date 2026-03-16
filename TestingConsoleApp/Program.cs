using Business.Commands;
using Data.Dtos;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WF.Cqrs.Crud.Business.Commands;
using WF.Cqrs.Crud.Business.Queries;
using WF.Cqrs.Mediator;
using WF.Data.Context;

namespace TestingConsoleApp;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var serviceColletion = new ServiceCollection();

        serviceColletion.AddServices();
        var serviceProvider = serviceColletion.BuildServiceProvider();
        using var serviceProviderScope = serviceProvider.CreateScope();
        
        var dbContext = serviceProviderScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        
        var mediator = serviceProviderScope.ServiceProvider.GetRequiredService<IMediator>();

        var gigione = await mediator.RunAsync(new GetAllPartsCommand());
        Console.WriteLine("retrieved all parts with the simple command");
        foreach (var gigi in gigione)
        {
            Console.WriteLine(gigi);
        }
        
        
        var partOne = new PartDtoSummary()
        {
            Location = "qui",
            Quantity = 4,
            Size1 = 4,
            Size2 = 44,
            Type = "toco",
        };

        var id = await mediator.RunAsync(new CreateEntityCommand<Part, PartDtoSummary>(partOne));
        Console.WriteLine($"created part one with id {id}");
        
        var partTwo = new PartDtoSummary()
        {
            Location = "Lì",
            Quantity = 3,
            Size1 = 33,
            Size2 = 33,
            Type = "mazza",
        };
        id = await mediator.RunAsync(new CreateEntityCommand<Part, PartDtoSummary>(partTwo));
        Console.WriteLine($"created part one with id {id}");

        var allParts = await mediator.RunAsync(new GetEntitiesQuery<Part, PartDtoSummary>());
        
        Console.WriteLine($"Retrieved all parts");
        foreach (var part in allParts)
        {
            Console.WriteLine(part);
        }

        var partDetail = await mediator.RunAsync(new GetEntityQuery<Part, PartDto>(id));
        Console.WriteLine($"Retrieved part with id {id}");
        Console.WriteLine(partDetail);

        partDetail.Size2 = 777;
        id = await mediator.RunAsync(new UpdateEntityCommand<Part, PartDto>(id, partDetail));
        Console.WriteLine($"updated part one with id {id}, now reading it again....");
        
        partDetail = await mediator.RunAsync(new GetEntityQuery<Part, PartDto>(id));
        Console.WriteLine($"Retrieved part with id {id}");
        Console.WriteLine(partDetail);

        await mediator.RunAsync(new DeleteEntityCommand<Part>(id));
        Console.WriteLine($"Deleted part with id {id}. Retrieving all parts again...");
        
        allParts = await mediator.RunAsync(new GetEntitiesQuery<Part, PartDtoSummary>());
        
        Console.WriteLine($"Retrieved all parts");
        foreach (var part in allParts)
        {
            Console.WriteLine(part);
        }
        
        Console.WriteLine("Goodbye, World!");
    }
}