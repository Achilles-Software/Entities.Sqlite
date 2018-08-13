#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Relational.Modelling.Mapping;
using Achilles.Entities.Relational.Modelling.Mapping.Builders;
using System;

#endregion

namespace Achilles.Entities.Relational.Modelling
{
    /// <summary>
    /// <see cref="IRelationalModelBuilder"/> implementation of the fluent builder inteface for database modelling.
    /// </summary>
    public class RelationalModelBuilder : IRelationalModelBuilder
    {
        #region Constructor(s)

        /// <summary>
        /// Constructs a new instance of <see cref="RelationalModelBuilder"/>.
        /// </summary>
        public RelationalModelBuilder()
        {
            EntityMappings = new EntityMappingCollection();
        }

        #endregion

        #region Public Properties

        /// <inheritdoc/>
        public EntityMappingCollection EntityMappings { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc />
        public void Entity<TEntity>( Action<IEntityMappingBuilder<TEntity>> action ) where TEntity : class
        {
            var builder = new EntityMappingBuilder<TEntity>();

            action( builder );
            AddMap( builder );
        }

        /// <inheritdoc />
        public IRelationalModel Build( DataContext context )
        {
            //foreach ( var entitySet in context.EntitySets )
            //{
            //    EntityMappings.GetOrAddMapping( entitySet.GetType() );
            //}

            context.OnModelBuilding( this );

            return new RelationalModel( EntityMappings );
        }

        #endregion

        #region Private Methods

        private void AddMap<TEntity>( IEntityMappingBuilder<TEntity> EntityMappingBuilder )
        {
            var EntityMapping = EntityMappingBuilder.Build();

            EntityMappings.Add( typeof( TEntity ), EntityMapping );
        }

        #endregion
    }
}
