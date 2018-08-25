#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Achilles.Entities.Linq
{
    public sealed class EntityCollection<TEntity> : IEntityCollection<TEntity>, IEntitySource, IEntityCollection, ICollection<TEntity>, IListSource
        where TEntity : class
    {
        #region Private Fields

        private EntitySet<TEntity> _source;
        private List<TEntity> _entities;

        private object _foreignKeyValue;
        private string _referenceKey;

        private bool _isLoaded;

        #endregion

        #region Constructor(s)

        public EntityCollection()
        {
        }

        #endregion

        #region Internal IEntitySource Implementation

        bool IEntitySource.HasSource => (_source != null);

        void IEntitySource.SetSource( IEntitySet source, string referenceKey, object foreignKeyValue )
        {
            _source = source as EntitySet<TEntity>;
            _referenceKey = referenceKey;
            _foreignKeyValue = foreignKeyValue;
        }

        #endregion

        #region Private Properties

        private List<TEntity> Entities
        {
            get
            {
                if ( !_isLoaded )
                {
                    Load();
                }

                return _entities;
            }
        }

        #endregion

        #region ICollection<TEntity> Implementation

        public int Count => ((ICollection<TEntity>)Entities).Count;

        public bool IsReadOnly => ((ICollection<TEntity>)Entities).IsReadOnly;

        public void Add( TEntity item )
        {
            ((ICollection<TEntity>)Entities).Add( item );
        }

        public void Clear()
        {
            ((ICollection<TEntity>)Entities).Clear();
        }

        public bool Contains( TEntity item )
        {
            return ((ICollection<TEntity>)Entities).Contains( item );
        }

        public void CopyTo( TEntity[] array, int arrayIndex )
        {
            ((ICollection<TEntity>)Entities).CopyTo( array, arrayIndex );
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return ((ICollection<TEntity>)Entities).GetEnumerator();
        }

        public bool Remove( TEntity item )
        {
            return ((ICollection<TEntity>)Entities).Remove( item );
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<TEntity>)Entities).GetEnumerator();
        }

        #endregion

        #region IListSource Implementation

        bool IListSource.ContainsListCollection => throw new NotImplementedException();

        public bool IsLoaded => _isLoaded;

        IList IListSource.GetList()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Implementation

        private void Load()
        {
            if ( !_isLoaded && _source != null )
            {
                _entities = _source.Where( FilterByForeignKeyPredicate( _foreignKeyValue ) ).ToList();
                _isLoaded = true;
            }
        }

        private Expression<Func<TEntity, bool>> FilterByForeignKeyPredicate( object foreignKey )
        {
            var entity = Expression.Parameter( typeof( TEntity ), "e" );
            var referenceKey = Expression.Property( entity, _referenceKey );
            var value = Expression.Constant( foreignKey, referenceKey.Type );
            var body = Expression.Equal( referenceKey, value );

            return Expression.Lambda<Func<TEntity, bool>>( body, entity );
        }

        #endregion
    }
}
