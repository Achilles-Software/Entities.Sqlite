#region Namespaces

using System;
using System.Linq.Expressions;

#endregion

namespace Achilles.Entities.Mapping.Builders
{
    public interface IEntityMappingBuilder<TEntity>
    {
        IEntityMapping EntityMapping { get; }

        /// <summary>
        /// Returns a <see cref="IPropertyMappingBuilder"/> instance to configure 
        /// the mapping for the specified property of <typeparamref name="TEntity"/>.
        /// </summary>
        /// <param name="mapping">An epxression which represents the property to be mapped.</param>
        /// <returns>A <see cref="IPropertyMappingBuilder"/> instance.</returns>
        IPropertyMappingBuilder Property( Expression<Func<TEntity, object>> mapping );

        IIndexMappingBuilder Index( Expression<Func<TEntity, object>> mapping );

        IAssociationMappingBuilder Relationship( Expression<Func<TEntity, object>> mapping );

        /// <summary>
        /// Specifies whether the <see cref="IEntityMapping"/> is case-sensitive.
        /// </summary>
        void IsCaseSensitive( bool caseSensitive );

        void ToTable( string tableName );

        /// <summary>
        /// Builds the <see cref="IEntityMapping"/> instance from this builder instance.
        /// </summary>
        /// <returns>A <see cref="IEntityMapping"/> instance.</returns>
        IEntityMapping Build();
    }
}
