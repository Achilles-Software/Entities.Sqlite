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
    /// Provides a fluent interface for configuring/building entities of <typeparamref name="TEntity"/>.
    /// </summary>
    /// <typeparam name="TEntity">The entity type to configure.</typeparam>
    public interface IEntityMappingBuilder<TEntity>
    {
        /// <summary>
        /// Gets the mapping for entity. 
        /// </summary>
        IEntityMapping EntityMapping { get; }

        /// <summary>
        /// Returns a <see cref="IColumnMappingBuilder"/> instance to configure 
        /// the mapping for the specified property of <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="property">An expression which represents the property to be mapped.</param>
        /// <returns>A <see cref="IColumnMappingBuilder"/> instance.</returns>
        IColumnMappingBuilder Column( Expression<Func<TEntity, object>> property );

        /// <summary>
        /// Returns a <see cref="IIndexMappingBuilder"/> instance to configure 
        /// the index for the specified property of <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="indexProperty">An expression which represents the property to be indexed.</param>
        /// <returns>A <see cref="IIndexMappingBuilder"/> instance.</returns>
        IIndexMappingBuilder HasIndex( Expression<Func<TEntity, object>> indexProperty );

        /// <summary>
        /// Returns a <see cref="IHasManyMappingBuilder"/> instance to configure 
        /// the one to many relationship for the specified property of <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="relationalProperty">An expression which represents the property.</param>
        /// <returns>A <see cref="IHasManyMappingBuilder"/> instance.</returns>
        IHasManyMappingBuilder HasMany( Expression<Func<TEntity, object>> relationalProperty );

        /// <summary>
        /// Returns a <see cref="IHasManyMappingBuilder"/> instance to configure 
        /// the one to one relationship for the specified property of <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="relationalProperty">An expression which represents the property.</param>
        /// <returns>A <see cref="IHasManyMappingBuilder"/> instance.</returns>
        IHasOneMappingBuilder<TEntity> HasOne( Expression<Func<TEntity, object>> relationalProperty );

        /// <summary>
        /// Specifies whether the <see cref="IEntityMapping"/> is case-sensitive.
        /// </summary>
        void IsCaseSensitive( bool caseSensitive );

        /// <summary>
        /// Sets the database table name.
        /// </summary>
        /// <param name="tableName">The database table name.</param>
        void ToTable( string tableName );

        /// <summary>
        /// Builds the <see cref="IEntityMapping"/> instance from this builder instance.
        /// </summary>
        /// <returns>A <see cref="IEntityMapping"/> instance.</returns>
        IEntityMapping Build();
    }
}
