using Microsoft.EntityFrameworkCore;

namespace WF.Data.Relational.Context;

public class ApplicationDbContext(IDbContextConfigurator dbContextConfigurator) : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => dbContextConfigurator.OnDbContextConfiguring(optionsBuilder);
}