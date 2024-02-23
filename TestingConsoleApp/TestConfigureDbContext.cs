using Data.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace TestingConsoleApp;

public class TestConfigureDbContext : IConfigureDbContext
{
    public void OnDbContextConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //throw new NotImplementedException(); TODO
    }
}