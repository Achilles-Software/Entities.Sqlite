#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Reflection;
using System;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping.Builders
{
    public class HasOneMappingBuilder<TEntity> : IHasOneMappingBuilder<TEntity>
    {
        private ForeignKeyBuilder _foreignKeyBuilder;

        public HasOneMappingBuilder( PropertyInfo propertyInfo )
        {
            Property = propertyInfo;
            ForeignKey = CreateForeignKeyMapping( propertyInfo );
        }

        private IForeignKeyMapping ForeignKey { get; }

        public PropertyInfo Property { get; }

        public IForeignKeyBuilder WithForeignKey( Expression<Func<TEntity, object>> mapping )
        {
            // Resolve property info from expression
            var propertyInfo = (PropertyInfo)ReflectionHelper.GetMemberInfo( mapping );

            _foreignKeyBuilder = new ForeignKeyBuilder( propertyInfo );

            return _foreignKeyBuilder;
        }

        public IForeignKeyMapping Build()
        {
            throw new NotImplementedException();
        }

        protected virtual IForeignKeyMapping CreateForeignKeyMapping( PropertyInfo propertyInfo ) 
            => new ForeignKeyMapping( propertyInfo );
    }
}
