using Business.Mapper.Abstractions;
using Data.Dtos;
using Data.Entities;
using Microsoft.Extensions.DependencyInjection;
using TestingConsoleApp;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        var serviceColletion = new ServiceCollection();

        serviceColletion.AddServices();
        var serviceProvider = serviceColletion.BuildServiceProvider();
        var mapper = serviceProvider.GetRequiredService<ApplicationMapper>();

        var part = new Part()
        {
            Id = 15,
            Location = "gigi",
            Quantity = 44,
            Size1 = 15,
            Type = "tocodelegno",
        };

        var partDto = mapper.Map<PartDto>(part);

        var partAgain = mapper.Map<Part>(partDto);


        Console.WriteLine("Goodbye, World!");
    }
}