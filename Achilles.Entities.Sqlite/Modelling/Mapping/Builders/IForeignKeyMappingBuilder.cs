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
    /// ...
    /// </summary>
    public interface IForeignKeyMappingBuilder
    {
        /// <summary>
        /// Gets the <see cref="PropertyInfo"/> instance associated with this builder.
        /// </summary>
        //PropertyInfo ForeignKey { get; }

        /// <summary>
        /// Sets the foreign key relationship name.
        /// </summary>
        /// <param name="relationshipName">The foreign key constraint name.</param>
        /// <returns>A <see cref="IForeignKeyMappingBuilder"/> instance.</returns>
        IForeignKeyMappingBuilder Name( string relationshipName );

        /// <summary>
        /// Sets the foreign key reference table and key. 
        /// </summary>
        /// <typeparam name="TRefEntity">The reference table entity.</typeparam>
        /// <param name="referenceKey">The reference table key predicate.</param>
        /// <returns>A <see cref="IForeignKeyMappingBuilder"/> instance.</returns>
        IForeignKeyMappingBuilder References<TRefEntity>( Expression<Func<TRefEntity, object>> referenceKey ) where TRefEntity : class;

        /// <summary>
        /// Sets the foreign key to cascade on delete.
        /// </summary>
        /// <returns>A <see cref="IForeignKeyMappingBuilder"/> instance.</returns>
        IForeignKeyMappingBuilder CascadeDelete();

        /// <summary>
        /// Builds the ForeignKeyMapping.
        /// </summary>
        /// <returns></returns>
        //IForeignKeyMapping Build();
    }
}
