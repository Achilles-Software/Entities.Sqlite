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

namespace Achilles.Entities.Relational.Modelling.Mapping.Builders
{
    public class HasOneMappingBuilder<TEntity> : IHasOneMappingBuilder<TEntity>
    {
        public PropertyInfo Property => throw new NotImplementedException();

        public IForeignKeyBuilder WithForeignKey( Expression<Func<TEntity, object>> mapping )
        {
            throw new NotImplementedException();
        }

        public IForeignKeyMapping Build()
        {
            throw new NotImplementedException();
        }
    }
}
