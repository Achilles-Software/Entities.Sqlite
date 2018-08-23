#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Extensions;
using Achilles.Entities.Reflection;
using System;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping.Builders
{
    /// <summary>
    /// Implements the <see cref="IHasManyMappingBuilder"/> interface to fluently build
    /// a 1-many foreign key relationship.
    /// </summary>
    public class HasManyMappingBuilder : IHasManyMappingBuilder
    {
        #region Private Fields

        private ForeignKeyMappingBuilder _foreignKeyMappingBuilder;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Constructs a HasManyMappingBuilder from the propertyInfo paarameter.
        /// </summary>
        /// <param name="relationshipProperty">The HasMany relationship property or field.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the relationship property or field is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the relationship property or field type is not an <see cref="EntityCollection{TEntity}"/>.
        /// </exception>
        public HasManyMappingBuilder( MemberInfo relationshipProperty )
        {
            RelationshipProperty = relationshipProperty ?? throw new ArgumentNullException( nameof( relationshipProperty ) );

            if ( RelationshipProperty.GetPropertyType().GetInterface( nameof( IEntityCollection ) ) == null )
            {
                throw new ArgumentException( nameof( relationshipProperty ) );
            }
        }

        #endregion

        #region Public API

        // <inheritsdoc/>
        public IForeignKeyMappingBuilder WithForeignKey<TEntity>( Expression<Func<TEntity, object>> foreignKeyLambda )
        {
            var foreignKey = ReflectionHelper.GetMemberInfo( foreignKeyLambda );

            _foreignKeyMappingBuilder = new ForeignKeyMappingBuilder( foreignKey );

            return _foreignKeyMappingBuilder;
        }

        #endregion

        #region Private/Internal Properties and Methods

        /// <summary>
        /// Gets the 1-1 relationship property<see cref="MemberInfo"/> instance associated with this builder.
        /// </summary>
        internal MemberInfo RelationshipProperty { get; }

        /// <summary>
        /// Builds a <see cref="IRelationshipMapping"/>.
        /// </summary>
        /// <returns>A <see cref="IRelationshipMapping"/> instance.</returns>
        internal RelationshipMapping Build( IEntityMapping entityMapping )
        {
            return new RelationshipMapping()
            {
                RelationshipProperty = RelationshipProperty,
                ForeignKeyMapping = _foreignKeyMappingBuilder.Build(),
                IsMany = true,
            };
        }

        #endregion
    }
}
