#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Relational.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Achilles.Entities
{
    /// <summary>
    /// Represents a database table.
    /// </summary>
    /// <typeparam name="TEntity">The type of entities stored in the database table.</typeparam>
    public class Entity<TEntity> : IQueryable<TEntity>
        where TEntity : class
    {
        #region Fields

        private readonly DbContext _context;
        private EntityQueryable<TEntity> _entityQueryable;

        #endregion

        #region Constructor(s)

        public Entity( DbContext context )
        {
            _context = context;
        }

        #endregion

        #region Public Properties

        public DbContext DbContext => _context;

        #endregion

        #region Public CRUD Methods

        public int Add( TEntity entity ) 
            => _context.Add( entity );

        public Task<int> AddAsync( TEntity entity, CancellationToken cancellationToken = default )
           => _context.AddAsync( entity, cancellationToken );

        public int Update( TEntity entity )
        => _context.Update( entity );

        public Task<int> UpdateAsync( TEntity entity, CancellationToken cancellationToken = default )
           => _context.UpdateAsync( entity, cancellationToken );

        public void Delete( TEntity entity )
            => _context.Delete( entity );

        public Task<int> DeleteAsync( TEntity entity, CancellationToken cancellationToken = default )
           => _context.DeleteAsync( entity, cancellationToken );

        #endregion

        #region Private IQueryable Methods

        private EntityQueryable<TEntity> EntityQueryable
        {
            get
            {
                if ( _entityQueryable == null )
                {
                    _entityQueryable = new EntityQueryable<TEntity>( _context );
                }

                return _entityQueryable;
            }
        }

        Type IQueryable.ElementType => EntityQueryable.ElementType;

        Expression IQueryable.Expression => EntityQueryable.Expression;

        IQueryProvider IQueryable.Provider => EntityQueryable.Provider;

        public IEnumerator<TEntity> GetEnumerator() => EntityQueryable.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => EntityQueryable.GetEnumerator();

        #endregion
    }
}
