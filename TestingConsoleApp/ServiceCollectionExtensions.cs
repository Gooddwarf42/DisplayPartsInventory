using Business.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace TestingConsoleApp;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
        => services.AddBusiness<TestConfigureDbContext>();
}
