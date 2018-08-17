#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using System;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping.Builders
{
    /// <summary>
    /// Provides a fluent interface for configuring/building a one to many relationship.
    /// </summary>
    public interface IHasManyMappingBuilder
    {
        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> instance associated with this builder.
        /// </summary>
        //PropertyInfo Property { get; }

        /// <summary>
        /// Provides a fluent builder interface for configuring a foreign key from an <typeparamref name="TEntity"/> type.
        /// </summary>
        /// <typeparam name="TEntity">The entity type that contains the foreign key reference.</typeparam>
        /// <param name="mapping"></param>
        /// <returns>A foreign key mapping builder.</returns>
        IForeignKeyMappingBuilder WithForeignKey<TEntity>( Expression<Func<TEntity, object>> mapping );

        /// <summary>
        /// Builds the <see cref="IForeignKeyMapping"/> instance from this builder instance.
        /// </summary>
        /// <returns>A <see cref="IForeignKeyMapping"/> instance.</returns>
        //IForeignKeyMapping Build( IEntityMapping entityMapping );
    }
}
