#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Modelling.Mapping;
using System;
using System.Collections.Generic;

#endregion

namespace Achilles.Entities.Modelling
{
    public class EntityModel : IEntityModel
    {
        #region Private Fields

        private readonly EntityMappingCollection _entityMappings;

        #endregion

        #region Constructor(s)

        public EntityModel( EntityMappingCollection entityMappings )
        {
            _entityMappings = entityMappings ?? throw new ArgumentNullException( nameof( entityMappings ) );
        }

        #endregion

        #region Public API

        public IReadOnlyCollection<IEntityMapping> EntityMappings => _entityMappings.Values as IReadOnlyCollection<IEntityMapping>;

        //public IEntityMapping GetOrAddEntityMapping( Type entityType )
        //{
        //    IEntityMapping mapping;

        //    if ( !_entityMappings.TryGetValue( entityType, out mapping ) )
        //    {
        //        var EntityMappingType = typeof( EntityMapping<> );
        //        var mapType = EntityMappingType.MakeGenericType( entityType );

        //        mapping = Activator.CreateInstance( mapType ) as IEntityMapping;

        //        _entityMappings[ entityType ] = mapping;
        //    }

        //    return mapping;
        //}

        public IEntityMapping GetEntityMapping<TEntity>() where TEntity : class
        {
            return GetEntityMapping( typeof( TEntity ) );
        }

        public IEntityMapping GetEntityMapping( Type entityType ) => _entityMappings.GetEntityMapping( entityType );

        #endregion
    }
}
