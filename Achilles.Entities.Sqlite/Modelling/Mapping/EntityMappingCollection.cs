#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

#endregion

namespace Achilles.Entities.Modelling.Mapping
{
    /// <summary>
    /// Collection class for entity mappings. 
    /// </summary>
    internal class EntityMappingCollection
    {
        #region Fields

        private EntityModel _model;
        private ConcurrentDictionary<Type, IEntityMapping> _entityMappings = new ConcurrentDictionary<Type, IEntityMapping>();

        #endregion

        #region Constructor(s)

        public EntityMappingCollection( EntityModel model )
        {
            _model = model;
        }

        #endregion

        /// <summary>
        /// Gets an entity type if it is already in the collection or creates and adds the entity to the collection.
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public IEntityMapping GetOrAddEntityMapping( Type entityType )
        {
            if ( !_entityMappings.TryGetValue( entityType, out IEntityMapping mapping ) )
            {
                var entityMappingType = typeof( EntityMapping<> );
                var genericEntityMappingType = entityMappingType.MakeGenericType( entityType );

                mapping = Activator.CreateInstance( genericEntityMappingType, _model ) as IEntityMapping;

                _entityMappings[ entityType ] = mapping;
            }

            return mapping;
        }

        public bool TryGetEntityMapping( Type entityType, out IEntityMapping entityMapping )
        {
            return _entityMappings.TryGetValue( entityType, out entityMapping );
        }

        public void TryAddEntityMapping( Type entityType, IEntityMapping entityMapping )
        {
            // TJT: Review this method.

            if ( _entityMappings.TryAdd( entityType, entityMapping ) )
            {
                // Compile the defined mapping into a column <-> member info lookup dictionary.
                //entityMapping.Compile();
            }
        }

        public IEntityMapping GetEntityMapping( Type entityType )
        {
            if ( _entityMappings.TryGetValue( entityType, out var entityMapping ) )
            {
                return entityMapping;
            }

            // FIXME:

            // TJT: This seems pretty harsh. Doesn't work with projections in materializer.

            throw new ArgumentException( nameof( entityType ) );
        }

        public ICollection<Type> Keys => ((IDictionary<Type, IEntityMapping>)_entityMappings).Keys;

        public ICollection<IEntityMapping> Values => ((IDictionary<Type, IEntityMapping>)_entityMappings).Values;

    }
}
