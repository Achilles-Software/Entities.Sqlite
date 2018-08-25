#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace Achilles.Entities.Linq
{
    public sealed class EntityReference<TEntity> : IEntityReference<TEntity>, IEntityReference, IEntityReferenceSource
        where TEntity : class
    {
        #region Private Fields

        private EntitySet<TEntity> _source;
        private TEntity _entity;
        private object _foreignKeyValue;
        private string _referenceKey;

        // TJT: Not yet sure where the expression predicate should live. 
        // Determine during beta optimization tasks
        //private Func<TEntity, bool> _filterByFkPredicate;

        private bool _isLoaded = false;
        
        #endregion

        #region Constructor(s)

        public EntityReference()
        {
        }

        #endregion

        #region Public API

        public bool IsLoaded => _isLoaded;

        public TEntity Value
        {
            get
            {
                if ( !_isLoaded )
                {
                    Load();
                }

                return _entity;
            }
        }

        #endregion

        #region Internal IEntityReferenceSource API

        bool IEntityReferenceSource.HasSource => (_source != null);

        void IEntityReferenceSource.SetSource( IEntitySet source, string referenceKey, object foreignKeyValue )
        {
            _source = source as EntitySet<TEntity>;
            _referenceKey = referenceKey;
            _foreignKeyValue = foreignKeyValue;
        }

        #endregion

        #region Private Implementation

        private void Load()
        {
            if ( !_isLoaded && _source != null )
            {
                _entity = _source.SingleOrDefault( FilterByForeignKeyPredicate( _foreignKeyValue ) );
                _isLoaded = true;
            }
        }

        private Expression<Func<TEntity,bool>> FilterByForeignKeyPredicate( object foreignKey )
        { 
            var entity = Expression.Parameter( typeof(TEntity), "e" );
            var referenceKey = Expression.Property( entity, _referenceKey );
            var value = Expression.Constant( foreignKey, referenceKey.Type );
            var body = Expression.Equal( referenceKey, value );

            return Expression.Lambda<Func<TEntity, bool>>( body, entity );
        }

        #endregion
    }
}
