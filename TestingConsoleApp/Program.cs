using Business.Commands.Test;
using Cqrs.Mediator;
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

        var mediator = serviceProvider.GetRequiredService<IMediator>();

        var command1 = new AddNumbersCommand();
        var result1 = await mediator.RunAsync(command1);
        Console.WriteLine(result1);


        var command2 = new AddNumbersCommand(4);
        var result2 = await mediator.RunAsync(command2);
        Console.WriteLine(result2);

        var command3 = new AddNumbersCommand(1, 2, 3, 4, 5);
        var result3 = await mediator.RunAsync(command3);
        Console.WriteLine(result3);

        var number = new Number(42);
        var commandWithNoReturn = new IncrementNumberCommand(number);
        await mediator.RunAsync(commandWithNoReturn);
        Console.WriteLine(number.Value);

        Console.WriteLine("Goodbye, World!");
    }
}