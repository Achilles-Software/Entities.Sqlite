#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Extensions;
using Achilles.Entities.Linq;
using Achilles.Entities.Reflection;
using System;
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping.Builders
{
    /// <summary>
    /// Implements the <see cref="IHasManyMappingBuilder"/> interface to fluently build
    /// a 1-many foreign key relationship.
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class HasOneMappingBuilder<TEntity> : IHasOneMappingBuilder<TEntity> where TEntity : class
    {
        #region Private Fields

        private ForeignKeyMappingBuilder _foreignKeyMappingBuilder;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Constructs a HasOneMappingBuilder with the provided property to build the 1-1 relationship on.
        /// </summary>
        /// <param name="relationshipProperty">The relationship property or field <see cref="MemberInfo"/>.</param>
        /// /// <exception cref="ArgumentNullException">
        /// Thrown when the HasOne relationship property or field is null.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the HasOne relationship property or field is not an <see cref="EntityReference{TEntity}"/>.
        /// </exception>
        public HasOneMappingBuilder( MemberInfo relationshipProperty )
        {
            RelationshipProperty = relationshipProperty ?? throw new ArgumentNullException( nameof( relationshipProperty ) );

            if ( RelationshipProperty.GetPropertyType().GetInterface( nameof( IEntityReference ) ) == null )
            {
                throw new ArgumentException( nameof( relationshipProperty ) );
            }
        }

        #endregion

        #region Private, Internal Properties

        internal MemberInfo RelationshipProperty { get; }

        #endregion

        #region Public API

        /// <inheritdoc/>
        public IForeignKeyMappingBuilder WithForeignKey( Expression<Func<TEntity, object>> foreignKeyLambda )
        {
            var foreignKeyProperty = ReflectionHelper.GetMemberInfo( foreignKeyLambda );

            _foreignKeyMappingBuilder = new ForeignKeyMappingBuilder( foreignKeyProperty );

            return _foreignKeyMappingBuilder;
        }

        #endregion

        #region Private Methods

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
                IsMany = false
            };
        }

        #endregion
    }
}
