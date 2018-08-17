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
using System.Collections;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping.Builders
{
    public class HasOneMappingBuilder<TEntity> : IHasOneMappingBuilder<TEntity> where TEntity : class
    {
        #region Private Fields

        private ForeignKeyMappingBuilder _foreignKeyMappingBuilder;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Constructs a HasOneMappingBuilder with the provided property to build the 1-1 relationship on.
        /// </summary>
        /// <param name="relationship">The </param>
        public HasOneMappingBuilder( MemberInfo relationship )
        {
            Relationship = relationship ?? throw new ArgumentNullException( nameof( relationship ) );

            if ( Relationship.GetPropertyType().GetInterface( nameof(IEnumerable)) != null )
            {
                throw new ArgumentException( nameof( relationship ) );
            }
            
            ForeignKeyMapping = CreateForeignKeyMapping( relationship );
        }

        #endregion

        #region Private, Internal Properties

        private IForeignKeyMapping ForeignKeyMapping { get; }
        /// <inheritdoc/>
        internal MemberInfo Relationship { get; }

        #endregion

        #region Public Properties and Methods

        public Type PropertyType => Relationship.GetPropertyType();

        //public string PropertyName => Relationship.Name;

        /// <inheritdoc/>
        public IForeignKeyMappingBuilder WithForeignKey( Expression<Func<TEntity, object>> foreignKeyLambda )
        {
            var foreignKeyProperty = ReflectionHelper.GetMemberInfo( foreignKeyLambda );

            _foreignKeyMappingBuilder = new ForeignKeyMappingBuilder( foreignKeyProperty );

            return _foreignKeyMappingBuilder;
        }

        /// <inheritdoc/>
        internal IForeignKeyMapping Build( IEntityMapping entityMapping )
        {
            return _foreignKeyMappingBuilder.Build();
        }

        #endregion

        #region Private Methods

        private IForeignKeyMapping CreateForeignKeyMapping( MemberInfo relationshipInfo ) 
            => new ForeignKeyMapping( relationshipInfo );

        #endregion
    }
}
