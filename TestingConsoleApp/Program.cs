using Microsoft.Extensions.DependencyInjection;
using TestingConsoleApp;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var serviceColletion = new ServiceCollection();

        serviceColletion.AddServices();

    }
}