#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping.Accessors
{
    internal class ColumnAccessor<TEntity, TValue> : MemberAccessor<TEntity,TValue>
        where TEntity : class
    {
        private readonly MemberInfo _columnInfo;

        public ColumnAccessor( MemberInfo columnInfo )
            : base( columnInfo )
        {
            _columnInfo = columnInfo;
        }
    }
}
