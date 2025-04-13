using System.Reflection;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using WF.Data.Relational.Context;

namespace TestingConsoleApp;

public class TestDbContextConfigurator : IDbContextConfigurator
{
    public void OnDbContextConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //throw new NotImplementedException(); TODO
    }

    // TODO just a draft
    public IEnumerable<Assembly> GetEntityAssemblies()
    {
        yield return typeof(Part).Assembly;
    }
}