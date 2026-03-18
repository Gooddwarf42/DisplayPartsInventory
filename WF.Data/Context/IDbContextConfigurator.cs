using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace WF.Data.Context;

public interface IDbContextConfigurator
{
    public void OnDbContextConfiguring(DbContextOptionsBuilder optionsBuilder);
    public IEnumerable<Assembly> GetEntityAssemblies();
}