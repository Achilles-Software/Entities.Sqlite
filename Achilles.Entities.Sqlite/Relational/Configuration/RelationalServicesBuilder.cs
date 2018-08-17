#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Modelling;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

#endregion

namespace Achilles.Entities.Configuration
{
    public class RelationalServicesBuilder 
    {
        public RelationalServicesBuilder( IServiceCollection serviceCollection )
        {
            ServiceCollection = serviceCollection ?? throw new System.ArgumentNullException( nameof( serviceCollection ) );
        }

        public IServiceCollection ServiceCollection { get; }

        
        public virtual RelationalServicesBuilder TryAddCoreServices()
        {
            TryAddTransient<IEntityModelBuilder, EntityModelBuilder>();
            TryAddSingleton<IDataContextService, DataContextService>();

            return this;
        }

        public virtual RelationalServicesBuilder TryAddTransient<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
            => TryAdd( typeof( TService ), typeof( TImplementation ), ServiceLifetime.Transient );

        public virtual RelationalServicesBuilder TryAddSingleton<TService, TImplementation>()
            where TService : class
            where TImplementation : class, TService
            => TryAdd( typeof( TService ), typeof( TImplementation ), ServiceLifetime.Singleton );

        public virtual RelationalServicesBuilder TryAdd( Type serviceType, Type implementationType, ServiceLifetime serviceLifetime )
        {
            if ( serviceType == null )
            {
                throw new ArgumentNullException( nameof( serviceType ) );
            }

            if ( implementationType == null )
            {
                throw new ArgumentNullException( nameof( implementationType ) );
            }

            var serviceDescriptor = new ServiceDescriptor( serviceType, implementationType, serviceLifetime );

            ServiceCollection.TryAdd( serviceDescriptor );

            return this;
        }
    }
}
