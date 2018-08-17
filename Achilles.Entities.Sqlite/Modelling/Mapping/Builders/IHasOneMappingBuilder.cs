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
    public interface IHasOneMappingBuilder<TEntity>
    {
        /// <summary>
        /// Gets the 1-1 relationship <see cref="PropertyInfo"/> instance associated with this builder.
        /// </summary>
        //PropertyInfo RelationshipProperty { get; }

        /// <summary>
        /// Sets the foreign key.
        /// </summary>
        /// <param name="foreignKey">The foreign key column property from the TEntity being mapped.</param>
        /// <returns></returns>
        IForeignKeyMappingBuilder WithForeignKey( Expression<Func<TEntity, object>> foreignKey );

        /// <summary>
        /// Builds a <see cref="IForeignKeyMapping"/>.
        /// </summary>
        /// <returns>A <see cref="IForeignKeyMapping"/> instance.</returns>
        //IForeignKeyMapping Build( IEntityMapping  entityMapping );
    }
}
