#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping.Accessors
{
    internal class EntityReferenceAccessor<TEntity, TValue> : MemberAccessor<TEntity,TValue>
        where TEntity : class
    {
        MemberInfo _entityReferenceInfo;
        EntityReference<TEntity> _entityReference;

        public EntityReferenceAccessor( MemberInfo entityReferenceInfo )
            : base( entityReferenceInfo )
        {
            _entityReferenceInfo = entityReferenceInfo;
        }

        public Type EntityType => typeof( TEntity );

        public override object GetValue( TEntity entity )
        {
            return base.GetValue( entity );
        }

        public override void SetValue( TEntity entity, object value )
        {
            // The base.GetValue gets the entityReference<> class
            var entityReference = base.GetValue( entity ) as IEntityReferenceSource;

            //Type entityReferencePropertyType = entityReferenceProperty.GetType();

            //// Get the EntitySet<TEntity>
            //var entityReference = entityReferencePropertyType.GetGenericArguments().First();
            //var entitySet = _context.EntitySets[ entityReference ];


            if ( !entityReference.HasSource )
            {
                entityReference.SetSource( value as IEnumerable<TEntity> );
            }

            //base.SetValue( entity, value );
        }
    }
}
