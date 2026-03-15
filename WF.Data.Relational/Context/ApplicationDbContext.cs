using Microsoft.EntityFrameworkCore;

namespace WF.Data.Relational.Context;

public sealed class ApplicationDbContext(IDbContextConfigurator dbContextConfigurator) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => dbContextConfigurator.OnDbContextConfiguring(optionsBuilder);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var assembliesToScan = dbContextConfigurator.GetEntityAssemblies().ToArray();
        foreach (var assembly in assembliesToScan)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
        }

        /*
        var entityTypes = assembliesToScan.GetConcreteTypesExtending<IEntity>();

        var entityConfiguratorTypes = assembliesToScan.GetConcreteTypesExtending<IBaseEntityConfigurator>().ToArray();

        var modelBuilderEntityMethod = typeof(ModelBuilder)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Single(m => m is { Name: nameof(ModelBuilder.Entity), IsGenericMethod: true } && m.GetParameters().Length == 0);

        foreach (var entityType in entityTypes)
        {
            var configuratorParentType = typeof(BaseEntityConfigurator<>)
                .MakeGenericType(entityType);

            var configuratorType = entityConfiguratorTypes
                .SingleOrDefault(entityConfiguratorType => entityConfiguratorType.Extends(configuratorParentType));

            if (configuratorType is null)
            {
                throw new NotSupportedException($"No entity configurator found for {entityType.Name}");
            }

            // I can not really cast this and use it directly, since it has type BaseEntityConfigurator<entityType>
            var configuratorInstance = Activator.CreateInstance(configuratorType)!;
            var configureMethod = configuratorType.GetMethod(nameof(BaseEntityConfigurator<IEntity>.Configure));
            var entityTypeBuilder = (EntityTypeBuilder)modelBuilderEntityMethod
                .MakeGenericMethod(entityType)
                .Invoke(modelBuilder, [])!;

            configureMethod!.Invoke(configuratorInstance, [entityTypeBuilder]);
        }
        // this all works, but turns out is absoultely unneeded because there is the ApplyConfigurationsFromAssembly method, lmao
        */
    }
}