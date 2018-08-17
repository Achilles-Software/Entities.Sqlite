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
    public class EntityMappingCollection //: IDictionary<Type,IEntityMapping>
    {
        #region Fields

        private ConcurrentDictionary<Type, IEntityMapping> _entityMappings = new ConcurrentDictionary<Type, IEntityMapping>();

        #endregion

        /// <summary>
        /// TODO:
        /// </summary>
        /// <param name="entityType"></param>
        /// <returns></returns>
        public IEntityMapping GetOrAddEntityMapping( Type entityType )
        {
            IEntityMapping mapping;

            if ( !_entityMappings.TryGetValue( entityType, out mapping ) )
            {
                var EntityMappingType = typeof( EntityMapping<> );
                var mappingType = EntityMappingType.MakeGenericType( entityType );

                mapping = Activator.CreateInstance( mappingType ) as IEntityMapping;

                _entityMappings[ entityType ] = mapping;

                mapping.Compile();
            }

            return mapping;
        }

        public void TryAddEntityMapping( Type entityType, IEntityMapping entityMapping )
        {
            if ( _entityMappings.TryAdd( entityType, entityMapping ) )
            {
                // Compile the defined mapping into a column <-> member info lookup dictionary.
                entityMapping.Compile();
            }
        }

        #region IDictionary Implementation through _EntityMappingpings

        public IEntityMapping this[ Type key ] { get => ((IDictionary<Type, IEntityMapping>)_entityMappings)[ key ]; set => ((IDictionary<Type, IEntityMapping>)_entityMappings)[ key ] = value; }

        public ICollection<Type> Keys => ((IDictionary<Type, IEntityMapping>)_entityMappings).Keys;

        public ICollection<IEntityMapping> Values => ((IDictionary<Type, IEntityMapping>)_entityMappings).Values;

        public int Count => ((IDictionary<Type, IEntityMapping>)_entityMappings).Count;

        public bool IsReadOnly => ((IDictionary<Type, IEntityMapping>)_entityMappings).IsReadOnly;

        public void Add( KeyValuePair<Type, IEntityMapping> item )
        {
            ((IDictionary<Type, IEntityMapping>)_entityMappings).Add( item );
        }

        public void Clear()
        {
            ((IDictionary<Type, IEntityMapping>)_entityMappings).Clear();
        }

        public bool Contains( KeyValuePair<Type, IEntityMapping> item )
        {
            return ((IDictionary<Type, IEntityMapping>)_entityMappings).Contains( item );
        }

        public bool ContainsKey( Type key )
        {
            return ((IDictionary<Type, IEntityMapping>)_entityMappings).ContainsKey( key );
        }

        public void CopyTo( KeyValuePair<Type, IEntityMapping>[] array, int arrayIndex )
        {
            ((IDictionary<Type, IEntityMapping>)_entityMappings).CopyTo( array, arrayIndex );
        }

        public IEnumerator<KeyValuePair<Type, IEntityMapping>> GetEnumerator()
        {
            return ((IDictionary<Type, IEntityMapping>)_entityMappings).GetEnumerator();
        }

        public bool Remove( Type key )
        {
            return ((IDictionary<Type, IEntityMapping>)_entityMappings).Remove( key );
        }

        public bool Remove( KeyValuePair<Type, IEntityMapping> item )
        {
            return ((IDictionary<Type, IEntityMapping>)_entityMappings).Remove( item );
        }

        public bool TryGetValue( Type key, out IEntityMapping value )
        {
            return ((IDictionary<Type, IEntityMapping>)_entityMappings).TryGetValue( key, out value );
        }

        //IEnumerator IEnumerable.GetEnumerator()
        //{
        //    return ((IDictionary<Type, IEntityMapping>)_entityMappings).GetEnumerator();
        //}

        #endregion
    }
}
