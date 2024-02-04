using Microsoft.EntityFrameworkCore;

namespace Data.Infrastructure
{
    public class ApplicationDbContext(IConfigureDbContext configureDbContext) : DbContext
    {
        // NOTE: fields initialized with primary contsructore are NOT readonly!

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => configureDbContext.OnDbContextConfiguring(optionsBuilder);
    }
}