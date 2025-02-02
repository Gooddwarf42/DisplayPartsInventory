using Microsoft.EntityFrameworkCore;

namespace WF.Data.Relational.Context;

public class ApplicationDbContext(IDbContextConfigurator dbContextConfigurator) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => dbContextConfigurator.OnDbContextConfiguring(optionsBuilder);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // TODO
        // Scan entities assemblies
        // Get all concrete IEntity types
        // Loop on IEntity types
        // Get the (uniue) configurator type extending IEntityConfigurator<TEntity>
        // run the configurator.Configure(modelBuilder.Entity<TEntity>())
    }
}