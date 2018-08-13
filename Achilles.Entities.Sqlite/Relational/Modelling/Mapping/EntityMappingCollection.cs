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

namespace Achilles.Entities.Relational.Modelling.Mapping
{
    /// <summary>
    /// Collection class for entity mappings. 
    /// </summary>
    public class EntityMappingCollection: IDictionary<Type,IEntityMapping>
    {
        #region Fields

        private ConcurrentDictionary<Type, IEntityMapping> _EntityMappings = new ConcurrentDictionary<Type, IEntityMapping>();

        #endregion

        #region Public Methods

        public IEntityMapping GetOrAddMapping( Type entityType )
        {
            IEntityMapping mapping;

            if ( !_EntityMappings.TryGetValue( entityType, out mapping ) )
            {
                var EntityMappingType = typeof( EntityMapping<> );
                var mapType = EntityMappingType.MakeGenericType( entityType );

                mapping = Activator.CreateInstance( mapType ) as IEntityMapping;

                _EntityMappings[ entityType ] = mapping;
            }

            return mapping;
        }

        public IEntityMapping GetMapping<TEntity>() where TEntity : class
        {
            return GetOrAddMapping( typeof( TEntity ) );
        }

        #endregion
  
        #region IDictionary Implementation through _EntityMappingpings

        public IEntityMapping this[ Type key ] { get => ((IDictionary<Type, IEntityMapping>)_EntityMappings)[ key ]; set => ((IDictionary<Type, IEntityMapping>)_EntityMappings)[ key ] = value; }

        public ICollection<Type> Keys => ((IDictionary<Type, IEntityMapping>)_EntityMappings).Keys;

        public ICollection<IEntityMapping> Values => ((IDictionary<Type, IEntityMapping>)_EntityMappings).Values;

        public int Count => ((IDictionary<Type, IEntityMapping>)_EntityMappings).Count;

        public bool IsReadOnly => ((IDictionary<Type, IEntityMapping>)_EntityMappings).IsReadOnly;

        public void Add( Type entityType, IEntityMapping EntityMapping )
        {
            if ( _EntityMappings.TryAdd( entityType, EntityMapping ) )
            {
                // Compile the defined mapping into a column <-> property lookup dictionary.
                EntityMapping.Compile();
            }
        }

        public void Add( KeyValuePair<Type, IEntityMapping> item )
        {
            ((IDictionary<Type, IEntityMapping>)_EntityMappings).Add( item );
        }

        public void Clear()
        {
            ((IDictionary<Type, IEntityMapping>)_EntityMappings).Clear();
        }

        public bool Contains( KeyValuePair<Type, IEntityMapping> item )
        {
            return ((IDictionary<Type, IEntityMapping>)_EntityMappings).Contains( item );
        }

        public bool ContainsKey( Type key )
        {
            return ((IDictionary<Type, IEntityMapping>)_EntityMappings).ContainsKey( key );
        }

        public void CopyTo( KeyValuePair<Type, IEntityMapping>[] array, int arrayIndex )
        {
            ((IDictionary<Type, IEntityMapping>)_EntityMappings).CopyTo( array, arrayIndex );
        }

        public IEnumerator<KeyValuePair<Type, IEntityMapping>> GetEnumerator()
        {
            return ((IDictionary<Type, IEntityMapping>)_EntityMappings).GetEnumerator();
        }

        public bool Remove( Type key )
        {
            return ((IDictionary<Type, IEntityMapping>)_EntityMappings).Remove( key );
        }

        public bool Remove( KeyValuePair<Type, IEntityMapping> item )
        {
            return ((IDictionary<Type, IEntityMapping>)_EntityMappings).Remove( item );
        }

        public bool TryGetValue( Type key, out IEntityMapping value )
        {
            return ((IDictionary<Type, IEntityMapping>)_EntityMappings).TryGetValue( key, out value );
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IDictionary<Type, IEntityMapping>)_EntityMappings).GetEnumerator();
        }

        #endregion
    }
}
