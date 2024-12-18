using System.Reflection;

namespace Kladovka.Cqrs
{
    public static class MediatrRegistrar
    {
        public static IServiceCollection AddMediatRExtension(this IServiceCollection services, Assembly currentAssembly)
        {
            services.AddMediatR(conf =>
            {
                conf.RegisterServicesFromAssembly(currentAssembly);
            });
            return services;
        }
    }
}
