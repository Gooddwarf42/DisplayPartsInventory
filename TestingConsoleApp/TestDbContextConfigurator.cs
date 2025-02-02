using Microsoft.EntityFrameworkCore;
using WF.Data.Relational.Context;

namespace TestingConsoleApp;

public class TestDbContextConfigurator : IDbContextConfigurator
{
    public void OnDbContextConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //throw new NotImplementedException(); TODO
    }
}