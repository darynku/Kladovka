using MediatR;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace Kladovka.Contracts.Handlers.Extensions
{
    public static class HandlerExtension
    {
        public static IServiceCollection AddRequestsHandlers(this IServiceCollection services, Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(assembly);

            Type[] interfaces = [typeof(IQueryHandler<,>),  typeof(ICommandHandler<,>), typeof(ICommandHandler<>)];

            var handlersTypes = assembly.DefinedTypes
                .Where(t => !t.IsOpenGeneric() && t.IsConcrete())
                .Where(t => Array.Exists(t.GetInterfaces(), i => i.IsGenericType && interfaces.Contains(i.GetGenericTypeDefinition())))
                .ToList();

            foreach(var handlerType in  handlersTypes)
            {
                IEnumerable<Type> handlerInterfaces = handlerType.GetInterfaces()
                                                       .Where(i => i.IsGenericType
                                                            && (i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)
                                                            || i.GetGenericTypeDefinition() == typeof(IRequestHandler<>)));
                foreach (var handlerInteface in handlerInterfaces)
                {
                    services.TryAddScoped(handlerInteface, handlerType);
                }

            }
            return services;
            
        }

        private static bool IsConcrete(this Type type)
        {
            return !type.IsAbstract && !type.IsInterface;
        }

        private static bool IsOpenGeneric(this Type type)
        {
            return type.IsGenericTypeDefinition || type.ContainsGenericParameters;
        }

    }
}
