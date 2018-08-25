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
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping.Accessors
{
    internal class EntityCollectionAccessor<TEntity, TValue> : MemberAccessor<TEntity,TValue>
        where TEntity : class
    {
        MemberInfo _entityCollectionInfo;
        EntityMapping<TEntity> _entityMapping;

        public EntityCollectionAccessor( EntityMapping<TEntity> entityMapping, MemberInfo entityCollectionInfo )
            : base( entityCollectionInfo )
        {
            _entityMapping = entityMapping;
            _entityCollectionInfo = entityCollectionInfo;
        }

        public Type EntityType => typeof( TEntity );

        public override object GetValue( TEntity entity )
        {
            return base.GetValue( entity );
        }

        public override void SetValue( TEntity entity, object value )
        {
            // The base.GetValue gets the entityReference<TEntity> class
            var entityCollection = base.GetValue( entity ) as IEntitySource;

            // The The foreign key mapping comes from the value passed to this method
            IForeignKeyMapping foreignKeyMapping = (IForeignKeyMapping)value;

            var entityCollectionType = foreignKeyMapping.ForeignKeyProperty.DeclaringType;

            // TJT: Clean the EntitySet access up!
            var entitySetSource = _entityMapping.Model.DataContext.EntitySets[ entityCollectionType ];

            var keyName = foreignKeyMapping.ForeignKeyProperty.Name;
            var keyValue = _entityMapping.GetColumn( entity, foreignKeyMapping.ReferenceKeyProperty.Name );

            if ( !entityCollection.HasSource )
            {
                entityCollection.SetSource( entitySetSource, keyName, keyValue );
            }
            else
            {
                throw new InvalidOperationException( "Entity reference source already set." );
            }
        }
    }
}
