using Kladovka.Infrastructure.Database.Configurator;
using Kladovka.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Kladovka.Extension
{
    public static class DataAccessRegistrar
    {
        public static IServiceCollection AddDataAccess<TDbContext, TDbContextConfigurator>(this IServiceCollection services)
            where TDbContext : DbContext
            where TDbContextConfigurator : class, IDbContextOptionsConfigurator<TDbContext>
        {
            services.AddEntityFrameworkNpgsql()
               .AddDbContextPool<TDbContext>(Configure<TDbContext>);

            services
                .AddSingleton<IDbContextOptionsConfigurator<TDbContext>, TDbContextConfigurator>()
                .AddScoped<DbContext>(sp => sp.GetRequiredService<TDbContext>())
                .AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));

            return services;
        }

        private static void Configure<TDbContext>(IServiceProvider sp, DbContextOptionsBuilder dbOptions) where TDbContext : DbContext
        {
            var configurator = sp.GetRequiredService<IDbContextOptionsConfigurator<TDbContext>>();
            configurator.Configure((DbContextOptionsBuilder<TDbContext>)dbOptions);
        }

    }
}
