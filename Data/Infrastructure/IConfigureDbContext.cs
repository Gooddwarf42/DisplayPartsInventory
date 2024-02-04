using Microsoft.EntityFrameworkCore;

namespace Data.Infrastructure
{
    public interface IConfigureDbContext
    {
        public void OnDbContextConfiguring(DbContextOptionsBuilder optionsBuilder);
    }
}