#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Linq;
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
    /// Represents the set of queryable entities of a specific type stored in a database table.
    /// </summary>
    /// <typeparam name="TEntity">The type of entities stored in the database table.</typeparam>
    public class EntitySet<TEntity> : IQueryable<TEntity>, IEntitySet
        where TEntity : class
    {
        #region Fields

        private readonly DataContext _context;
        private EntityQueryable<TEntity> _entityQueryable;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Creates a new EntitySet instance and adds it to the <see cref="DataContext"/>.
        /// </summary>
        /// <param name="context">The <see cref="DataContext"/> that this EntitySet is added to.</param>
        public EntitySet( DataContext context )
        {
            _context = context ?? throw new ArgumentNullException( nameof( context ) );

            _context.AddEntitySet( this );
        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the <see cref="DataContext"/> that this EntitySet belongs to.
        /// </summary>
        public DataContext DataContext => _context;

        /// <inheritdoc/>
        public Type EntityType => typeof( TEntity );

        #endregion

        #region Public CRUD Methods

        // TODO: Add documented user interface

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
