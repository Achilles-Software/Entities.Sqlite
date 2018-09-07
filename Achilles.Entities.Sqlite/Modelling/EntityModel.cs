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
    /// <summary>
    /// Represents the entity data model for a given data context.
    /// </summary>
    public class EntityModel : IEntityModel
    {
        #region Private Fields

        private DataContext _context;
        private EntityMappingCollection _entityMappings;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Constructs a <see cref="EntityModel"/> instance for a data context.
        /// </summary>
        /// <param name="context">The data context.</param>
        public EntityModel( DataContext context )
        {
            _context = context;
            _entityMappings = new EntityMappingCollection( this );
        }

        #endregion

        #region Public API

        /// <inheritdoc/>
        public IReadOnlyCollection<IEntityMapping> EntityMappings => _entityMappings.Values as IReadOnlyCollection<IEntityMapping>;

        /// <inheritdoc/>
        public IEntityMapping GetEntityMapping<TEntity>() where TEntity : class
        {
            return GetEntityMapping( typeof( TEntity ) );
        }

        /// <inheritdoc/>
        public IEntityMapping GetEntityMapping( Type entityType ) => _entityMappings.GetEntityMapping( entityType );

        /// <inheritdoc/>
        public bool TryGetEntityMapping( Type entityType, out IEntityMapping entityMapping )
            => _entityMappings.TryGetEntityMapping( entityType, out entityMapping );
        
        #endregion

        #region Internal

        internal IEntityMapping GetOrAddEntityMapping( Type entityType ) 
            => _entityMappings.GetOrAddEntityMapping( entityType );

        internal void TryAddEntityMapping( Type entityType, IEntityMapping entityMapping ) 
            => _entityMappings.TryAddEntityMapping( entityType, entityMapping );

        internal DataContext DataContext => _context;

        #endregion
    }
}
