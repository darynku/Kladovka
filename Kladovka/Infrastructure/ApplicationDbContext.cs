using Kladovka.Domain;
using Kladovka.Infrastructure.Database.Configurator;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Kladovka.Infrastructure
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Product> Products => Set<Product>();   

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

    }
    public class ApplicationDbContextConfigurator(IConfiguration configuration, ILoggerFactory logger) 
        : BaseDbContextConfigurator<ApplicationDbContext>(configuration, logger)
    {
        protected override string ConnectionString => "Default";

    }

}
