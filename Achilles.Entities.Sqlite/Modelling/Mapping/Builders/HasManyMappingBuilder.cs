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
    /// Implements the <see cref="IHasManyMappingBuilder"/> interface to fluently configure
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
        /// <param name="relationshipInfo"></param>
        public HasManyMappingBuilder( MemberInfo relationshipInfo )
        {
            Relationship = relationshipInfo;
            ForeignKeyMapping = CreateForeignKeyMapping( relationshipInfo );
        }

        #endregion

        private IForeignKeyMapping ForeignKeyMapping { get; }

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        internal MemberInfo Relationship { get; }

        #endregion

        public Type PropertyType => Relationship.GetPropertyType();

        /// <inheritsdoc/>
        public IForeignKeyMappingBuilder WithForeignKey<TEntity>( Expression<Func<TEntity, object>> foreignKeyLambda )
        {
            var foreignKey = ReflectionHelper.GetMemberInfo( foreignKeyLambda );

            _foreignKeyMappingBuilder = new ForeignKeyMappingBuilder( foreignKey );

            return _foreignKeyMappingBuilder;
        }

        internal IForeignKeyMapping Build( IEntityMapping entityMapping ) => _foreignKeyMappingBuilder.Build();

        protected virtual IForeignKeyMapping CreateForeignKeyMapping( MemberInfo relationshipInfo ) 
            => new ForeignKeyMapping( relationshipInfo );
    }
}
