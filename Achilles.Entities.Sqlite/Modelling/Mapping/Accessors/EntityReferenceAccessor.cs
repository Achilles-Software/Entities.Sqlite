#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Extensions;
using Achilles.Entities.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping.Accessors
{
    internal class EntityReferenceAccessor<TEntity, TValue> : MemberAccessor<TEntity,TValue>
        where TEntity : class
    {
        MemberInfo _entityReferenceInfo;
        EntityMapping<TEntity> _entityMapping;

        public EntityReferenceAccessor( EntityMapping<TEntity> entityMapping, MemberInfo entityReferenceInfo )
            : base( entityReferenceInfo )
        {
            _entityMapping = entityMapping;
            _entityReferenceInfo = entityReferenceInfo;
        }

        public Type EntityType => typeof( TEntity );

        public override object GetValue( TEntity entity )
        {
            return base.GetValue( entity );
        }

        public override void SetValue( TEntity entity, object value )
        {
            // The base.GetValue gets the entityReference<TEntity> class
            var entityReference = base.GetValue( entity ) as IEntitySource;

            // The The foreign key mapping comes from the value passed to this method
            IForeignKeyMapping foreignKeyMapping = (IForeignKeyMapping)value;

            var entityReferenceType = foreignKeyMapping.ReferenceKeyProperty.DeclaringType;

            // TJT: Clean the EntitySet access up!
            var entitySetSource = _entityMapping.Model.DataContext.EntitySets[ entityReferenceType ];
            var referenceKeyName = foreignKeyMapping.ReferenceKeyProperty.Name;
            var foreignKeyValue = _entityMapping.GetForeignKey( entity, foreignKeyMapping.PropertyName );

            if ( !entityReference.HasSource )
            {
                entityReference.SetSource( entitySetSource, referenceKeyName, foreignKeyValue );
            }
            else
            {
                throw new InvalidOperationException( "Entity reference source already set." );
            }
        }
    }
}
