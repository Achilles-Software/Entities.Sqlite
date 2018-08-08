#region Copyright Notice

// Copyright (c) by Achilles Software, http://achilles-software.com
//
// The source code contained in this file may not be copied, modified, distributed or
// published by any means without the express written agreement by Achilles Software.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com
//
// All rights reserved.

#endregion

#region Namespaces

using Achilles.Entities.Relational.Query;
using Achilles.Entities.Storage;
using Remotion.Linq.Parsing.Structure;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Achilles.Entities
{
    public class DbTable<TEntity> : IQueryable<TEntity>
        where TEntity : class
    {
        #region Fields

        private readonly DbContext _context;
        private DbTableQueryable<TEntity> _dbTableQueryable;

        #endregion

        #region Constructor(s)

        public DbTable( DbContext context )
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

        private DbTableQueryable<TEntity> DbTableQueryable
        {
            get
            {
                if ( _dbTableQueryable == null )
                {
                    _dbTableQueryable = new DbTableQueryable<TEntity>( _context );
                }

                return _dbTableQueryable;
            }
        }

        Type IQueryable.ElementType => DbTableQueryable.ElementType;

        Expression IQueryable.Expression => DbTableQueryable.Expression;

        IQueryProvider IQueryable.Provider => DbTableQueryable.Provider;

        public IEnumerator<TEntity> GetEnumerator() => DbTableQueryable.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => DbTableQueryable.GetEnumerator();

        #endregion
    }
}
