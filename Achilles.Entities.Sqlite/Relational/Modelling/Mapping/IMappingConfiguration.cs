#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Mapping;
using Achilles.Entities.Mapping.Builders;
using System;

#endregion

namespace Achilles.Entities
{
    public interface IMappingConfiguration
    {
        /// <summary>
        /// Gets a collection of entity mappings.
        /// </summary>
        EntityMappingCollection EntityMappings { get; }

        /// <summary>
        /// Configures the mapping for <typeparamref name="TEntity"/>.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="action">A delegate which configures the mapping for <typeparamref name="TEntity"/>.</param>
        void Entity<TEntity>( Action<IEntityMappingBuilder<TEntity>> action ) where TEntity : class;

        /// <summary>
        /// Adds the specified <see cref="IEntityMappingBuilder{TEntity}"/> instance to this configuration instance.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <param name="EntityMappingBuilder">
        /// An instance of <see cref="IEntityMappingBuilder{TEntity}"/> which represents the mapping for <typeparamref name="TEntity"/>.
        /// </param>
        //void AddMap<TEntity>( IEntityMappingBuilder<TEntity> EntityMappingBuilder );
    }
}
