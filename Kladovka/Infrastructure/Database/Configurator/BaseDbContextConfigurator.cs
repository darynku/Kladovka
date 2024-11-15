using Microsoft.EntityFrameworkCore;

namespace Kladovka.Infrastructure.Database.Configurator
{
    public abstract class BaseDbContextConfigurator<TDbContext>(IConfiguration configuration, ILoggerFactory logger) 
        : IDbContextOptionsConfigurator<TDbContext> where TDbContext : DbContext
    {
        protected abstract string ConnectionString { get; }

        public void Configure(DbContextOptionsBuilder<TDbContext> optionsBuilder)
        {
            var connectionString = configuration.GetConnectionString(ConnectionString);
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new InvalidOperationException($"Incorrect connection string {ConnectionString}");
            }
            optionsBuilder.UseLoggerFactory(logger).UseNpgsql(connectionString);
        }
    }
}
