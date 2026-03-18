using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using WF.Data.Context;

namespace WF.Cqrs.Tests.Crud.ResolvedDependencies;

public class DbContextConfigurator : IDbContextConfigurator
{
    public void OnDbContextConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var path = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}-test.db");
        optionsBuilder.UseSqlite($"Data Source={path}");
    }

    public IEnumerable<Assembly> GetEntityAssemblies()
    {
        yield return typeof(DbContextConfigurator).Assembly;
    }
}