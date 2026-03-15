using System.Reflection;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using WF.Data.Relational.Context;

namespace TestingConsoleApp.ResolvedDependencies;

public class TestDbContextConfigurator : IDbContextConfigurator
{
    public void OnDbContextConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var path = Path.Combine(Path.GetTempPath(), "verySimpleTest.db");
        optionsBuilder.UseSqlite($"Data Source={path}");
    }
    
    public IEnumerable<Assembly> GetEntityAssemblies()
    {
        yield return typeof(Part).Assembly;
    }
}