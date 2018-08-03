#region Namespaces

using Achilles.Entities.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

#endregion

namespace Achilles.Entities
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDbContext<TContext>(
            this IServiceCollection serviceCollection,
            Action<DbContextOptionsBuilder<TContext>> optionsAction,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped )
            where TContext : DbContext
        {
            if ( serviceCollection == null )
            {
                throw new ArgumentNullException( nameof( serviceCollection ) );
            }

            if ( contextLifetime == ServiceLifetime.Singleton )
            {
                optionsLifetime = ServiceLifetime.Singleton;
            }

            serviceCollection.TryAdd(
                new ServiceDescriptor(
                    typeof( DbContextOptions<TContext> ),
                    p => DbContextOptionsFactory<TContext>( p, optionsAction ),
                    optionsLifetime ) );

            serviceCollection.Add(
                new ServiceDescriptor(
                    typeof( DbContextOptions ),
                    p => p.GetRequiredService<DbContextOptions<TContext>>(),
                    optionsLifetime ) );
            
            serviceCollection.TryAdd( new ServiceDescriptor( typeof( TContext ), typeof( TContext ), contextLifetime ) );

            return serviceCollection;
        }

        private static DbContextOptions<TContext> DbContextOptionsFactory<TContext>(
            IServiceProvider applicationServiceProvider,
            Action<DbContextOptionsBuilder<TContext>> optionsAction )
            where TContext : DbContext
        {
            var builder = new DbContextOptionsBuilder<TContext>(
                new DbContextOptions<TContext>() );

            optionsAction?.Invoke( builder );

            return builder.Options;
        }
    }
}
