using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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

        var partOne = new Part()
        {
            Location = "qui",
            Quantity = 4,
            Size1 = 4,
            Size2 = 44,
            Type = "toco",
        };

        dbContext.Add(partOne);
        await dbContext.SaveChangesAsync();

        var allParts = await dbContext.Set<Part>().ToListAsync();
        
        foreach (var part in allParts)
        {
            Console.WriteLine(part);
        }
        
        Console.WriteLine("Goodbye, World!");
    }
}