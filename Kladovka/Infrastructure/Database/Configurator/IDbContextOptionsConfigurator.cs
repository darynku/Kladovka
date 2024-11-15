using Microsoft.EntityFrameworkCore;

namespace Kladovka.Infrastructure.Database.Configurator
{
    public interface IDbContextOptionsConfigurator<TDbContext> where TDbContext : DbContext
    {
        void Configure(DbContextOptionsBuilder<TDbContext> optionsBuilder);
    }
}