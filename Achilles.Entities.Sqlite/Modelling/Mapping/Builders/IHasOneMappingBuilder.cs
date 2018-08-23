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

#endregion

namespace Achilles.Entities.Modelling.Mapping.Builders
{
    /// <summary>
    /// The public API methods used to fluently build a 1-One or HasOne relationship.
    /// </summary>
    /// <typeparam name="TEntity">The Entity type.</typeparam>
    public interface IHasOneMappingBuilder<TEntity>
    {
        /// <summary>
        /// Sets the foreign key.
        /// </summary>
        /// <param name="foreignKey">The foreign key column property from the TEntity being mapped.</param>
        /// <returns></returns>
        IForeignKeyMappingBuilder WithForeignKey( Expression<Func<TEntity, object>> foreignKey );
    }
}
