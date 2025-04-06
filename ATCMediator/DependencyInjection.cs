
using System.Reflection;
using ATCMediator.Mediator.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace ATCMediator
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddATCMediator(this IServiceCollection services, params Assembly[] assemblies)
        {
            services.AddScoped<IMediator, Mediator.Mediator>();

            foreach (var assembly in assemblies)
            {
                // Registrar ICommandHandler<T>
                var commandHandlerType = typeof(ICommandHandler<>);
                var commandHandlers = assembly.GetTypes()
                    .Where(t => t.GetInterfaces().Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == commandHandlerType));

                foreach (var handler in commandHandlers)
                {
                    var @interface = handler.GetInterfaces()
                        .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == commandHandlerType);
                    services.AddScoped(@interface, handler);
                }

                // Registrar IQueryHandler<T,Q>
                var queryHandlerType = typeof(IQueryHandler<,>);
                var queryHandlers = assembly.GetTypes()
                    .Where(t => t.GetInterfaces().Any(i =>
                        i.IsGenericType && i.GetGenericTypeDefinition() == queryHandlerType));

                foreach (var handler in queryHandlers)
                {
                    var @interface = handler.GetInterfaces()
                        .First(i => i.IsGenericType && i.GetGenericTypeDefinition() == queryHandlerType);
                    services.AddScoped(@interface, handler);
                }
            }

            return services;
        }
    }
}