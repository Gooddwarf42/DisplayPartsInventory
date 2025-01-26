using Microsoft.Extensions.DependencyInjection;

namespace TestingConsoleApp;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var serviceColletion = new ServiceCollection();

        serviceColletion.AddServices();
        var serviceProvider = serviceColletion.BuildServiceProvider();

        Console.WriteLine("Goodbye, World!");
    }
}