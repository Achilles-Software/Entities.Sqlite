#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

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
        public static IServiceCollection AddDataContext<TContext>(
            this IServiceCollection serviceCollection,
            Action<DataContextOptionsBuilder<TContext>> optionsAction,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped,
            ServiceLifetime optionsLifetime = ServiceLifetime.Scoped )
            where TContext : DataContext
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
                    typeof( DataContextOptions<TContext> ),
                    p => DataContextOptionsFactory<TContext>( p, optionsAction ),
                    optionsLifetime ) );

            serviceCollection.Add(
                new ServiceDescriptor(
                    typeof( DataContextOptions ),
                    p => p.GetRequiredService<DataContextOptions<TContext>>(),
                    optionsLifetime ) );

            serviceCollection.TryAdd( new ServiceDescriptor( typeof( TContext ), typeof( TContext ), contextLifetime ) );

            return serviceCollection;
        }

        private static DataContextOptions<TContext> DataContextOptionsFactory<TContext>(
            IServiceProvider applicationServiceProvider,
            Action<DataContextOptionsBuilder<TContext>> optionsAction )
            where TContext : DataContext
        {
            var builder = new DataContextOptionsBuilder<TContext>(
                new DataContextOptions<TContext>() );

            optionsAction?.Invoke( builder );

            return builder.Options;
        }
    }
}
