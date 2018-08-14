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
    /// Implements the <see cref="IHasManyMappingBuilder"/> interface to fluently configure
    /// a 1-many foreign key relationship.
    /// </summary>
    public class HasManyMappingBuilder : IHasManyMappingBuilder
    {
        #region Constructor(s)

        public HasManyMappingBuilder( PropertyInfo propertyInfo )
        {
            Property = propertyInfo;
            ForeignKey = CreateForeignKeyMapping( propertyInfo );
        }

        #endregion

        private IForeignKeyMapping ForeignKey { get; }

        #region Public Properties

        /// <summary>
        /// 
        /// </summary>
        public PropertyInfo Property { get; }

        #endregion

        public IForeignKeyBuilder WithForeignKey<TEntity>( Expression<Func<TEntity, object>> mapping )
        {

            throw new NotImplementedException();
        }

        public IForeignKeyMapping Build()
        {
            throw new System.NotImplementedException();
        }

        protected virtual IForeignKeyMapping CreateForeignKeyMapping( PropertyInfo propertyInfo ) 
            => new ForeignKeyMapping( propertyInfo );
    }
}
