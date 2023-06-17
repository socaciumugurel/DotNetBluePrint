using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BluePrint.core.Infrastructure.RegistrationStrategies
{
    /// <summary>
    /// Strategy how to register Services implementation in ServiceCollection
    /// </summary>
    public class ServicesRegistrationStrategy : IStrategy<IServiceCollection>
    {
        private readonly IEnumerable<Assembly> _assemblies;

        /// <summary>
        /// Constructor ServicesRegistrationStrategy
        /// </summary>
        /// <param name="assemblies"></param>
        public ServicesRegistrationStrategy(IEnumerable<Assembly> assemblies)
        {
            _assemblies = assemblies;
        }

        /// <inheritdoc />
        public void Execute(IServiceCollection input)
        {
            var types = _assemblies.SelectMany(a => a.GetTypes());

            foreach (var type in types)
            {
                var registrations = GetServicesFrom(type);
                foreach (var reg in registrations)
                {
                    RegisterService(reg, input);
                }
            }
        }

        private void RegisterService(ServiceInfo registration, IServiceCollection serviceCollection)
        {
            _lifetimeBuilder[registration.InstanceLifetime](registration.From, registration.To, serviceCollection);
        }

        private static IEnumerable<ServiceInfo> GetServicesFrom(Type type)
        {
            var attributes = type.GetTypeInfo().GetAttributes<ServiceAttribute>(false);
            return attributes.Select(a => new ServiceInfo(a.ExportType ?? type, type, a.Lifetime));
        }

        private readonly IDictionary<Lifetime, Func<Type, Type, IServiceCollection, IServiceCollection>> _lifetimeBuilder =
            new Dictionary<Lifetime, Func<Type, Type, IServiceCollection, IServiceCollection>>
            {
                {
                    Lifetime.Singleton,
                    (serviceType, implementationType, container) =>
                        container.AddSingleton(serviceType, implementationType)
                },
                {
                    Lifetime.Transient,
                    (serviceType, implementationType, container) =>
                        container.AddTransient(serviceType, implementationType)
                },
                {
                    Lifetime.Scoped,
                    (serviceType, implementationType, container) =>
                        container.AddScoped(serviceType, implementationType)
                }
            };

        /// <summary>
        /// Static Factory
        /// </summary>
        /// <param name="assemblies"></param>
        /// <returns></returns>
        public static ServicesRegistrationStrategy Create(IEnumerable<Assembly> assemblies)
        {
            return new ServicesRegistrationStrategy(assemblies);
        }
    }
}
