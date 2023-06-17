using Microsoft.Extensions.DependencyInjection;

namespace BluePrint.core.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Execute<TStrategy>(this IServiceCollection containerBuilder)
            where TStrategy : IStrategy<IServiceCollection>, new()
        {
            return containerBuilder.Execute(() => new TStrategy());
        }

        public static IServiceCollection Execute<TStrategy>(this IServiceCollection containerBuilder,
            Func<TStrategy> strategyFactory) where TStrategy : IStrategy<IServiceCollection>
        {
            var strategy = strategyFactory();
            return containerBuilder.Execute(strategy);
        }
    }
}
