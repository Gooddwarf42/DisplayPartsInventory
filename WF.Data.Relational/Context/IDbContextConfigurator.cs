using Microsoft.EntityFrameworkCore;

namespace WF.Data.Relational.Context;

public interface IDbContextConfigurator
{
    public void OnDbContextConfiguring(DbContextOptionsBuilder optionsBuilder);
}