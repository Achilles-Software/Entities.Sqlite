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

#endregion

namespace Achilles.Entities
{
    public sealed class EntityCollection<TEntity> : ICollection<TEntity>, IListSource, IEntityCollection
        where TEntity : class
    {
        #region Private Fields

        private IEnumerable<TEntity> _source;
        private HashSet<TEntity> _entities;
        
        private bool _isLoaded;

        #endregion

        #region Constructor(s)

        public EntityCollection()
        {
            var test = 6;
        }

        #endregion

        #region ICollection<TEntity> Implementation

        public int Count => ((ICollection<TEntity>)_entities).Count;

        public bool IsReadOnly => ((ICollection<TEntity>)_entities).IsReadOnly;

        public void Add( TEntity item )
        {
            ((ICollection<TEntity>)_entities).Add( item );
        }

        public void Clear()
        {
            ((ICollection<TEntity>)_entities).Clear();
        }

        public bool Contains( TEntity item )
        {
            return ((ICollection<TEntity>)_entities).Contains( item );
        }

        public void CopyTo( TEntity[] array, int arrayIndex )
        {
            ((ICollection<TEntity>)_entities).CopyTo( array, arrayIndex );
        }

        public IEnumerator<TEntity> GetEnumerator()
        {
            return ((ICollection<TEntity>)_entities).GetEnumerator();
        }

        public bool Remove( TEntity item )
        {
            return ((ICollection<TEntity>)_entities).Remove( item );
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((ICollection<TEntity>)_entities).GetEnumerator();
        }

        #endregion

        #region IListSource Implementation

        bool IListSource.ContainsListCollection => throw new NotImplementedException();

        IList IListSource.GetList()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
