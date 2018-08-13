#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Relational.Modelling.Mapping;
using Achilles.Entities.Relational.Modelling.Mapping.Builders;
using System;

#endregion

namespace Achilles.Entities.Relational.Modelling
{
    /// <summary>
    /// An interface for building a database model.
    /// </summary>
    public interface IRelationalModelBuilder
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
        /// Builds a database model.
        /// </summary>
        /// <param name="context">The data context.</param>
        /// <returns>A relation model</returns>
        IRelationalModel Build( DataContext context );
    }
}
