using Data.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace Data.Extension
{
    public static class DataExtensions
    {
        // TODO; Why does this not work? I had done with a neat trick to do the thing with runtime types, but I can't see why this wouldn't work.
        public static IServiceCollection AddData<TConfigureDbContext>(this IServiceCollection services)
            where TConfigureDbContext : class, IConfigureDbContext
            => services.
                AddScoped<IConfigureDbContext, TConfigureDbContext>();
    }
}