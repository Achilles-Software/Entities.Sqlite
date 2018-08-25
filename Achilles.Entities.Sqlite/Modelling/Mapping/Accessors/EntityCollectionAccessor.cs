#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Achilles.Entities.Modelling.Mapping.Accessors
{
    internal class EntityCollectionAccessor<TEntity, TValue> : MemberAccessor<TEntity,TValue>
    {
        MemberInfo _entityCollectionInfo;
        IEntityMapping _entityMapping;

        public EntityCollectionAccessor( IEntityMapping entityMapping, MemberInfo entityReferenceInfo )
            : base( entityReferenceInfo )
        {
            _entityMapping = entityMapping;
            _entityCollectionInfo = entityReferenceInfo;
        }
    }
}
