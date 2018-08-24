#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Modelling.Mapping;
using Achilles.Entities.Modelling.Mapping.Builders;
using System;

#endregion

namespace Achilles.Entities.Modelling
{
    /// <summary>
    /// <see cref="IEntityModelBuilder"/> implementation of the fluent builder inteface for entity data modelling.
    /// </summary>
    public class EntityModelBuilder : IEntityModelBuilder
    {
        private DataContext _context;

        #region Constructor(s)

        /// <summary>
        /// Constructs a new instance of <see cref="EntityModelBuilder"/>.
        /// </summary>
        public EntityModelBuilder()
        {
        }

        #endregion

        #region Private Properties

        private EntityModel Model { get; set; }

        //private EntityMappingCollection EntityMappings => Model.EntityMappings;

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public void Entity<TEntity>( Action<IEntityMappingBuilder<TEntity>> action ) where TEntity : class
        {
            var builder = new EntityMappingBuilder<TEntity>( Model );

            action( builder );

            AddEntityMapping( builder );
        }

        /// <inheritdoc />
        public IEntityModel Build( DataContext context )
        {
            _context = context;

            Model = new EntityModel( context );

            // TJT: Should all entities be added in the user's data context constructor?
            // What should happen if an entity is referenced during model building that is
            // not in the context?

            foreach ( var entitySetType in context.EntitySets.Keys )
            {
                Model.GetOrAddEntityMapping( entitySetType );
            }

            context.OnModelBuilding( this );

            // The model entity mappings have now been configured.
            // Resolve Entity references and validate the model. 
            ResolveMappingsAndValidateModel();

            return Model;
        }

        #endregion

        #region Private Methods

        private void AddEntityMapping<TEntity>( EntityMappingBuilder<TEntity> entityMappingBuilder ) where TEntity : class
        {
            var EntityMapping = entityMappingBuilder.Build();

            Model.TryAddEntityMapping( typeof( TEntity ), EntityMapping );
        }

        private void ResolveMappingsAndValidateModel()
        {
            foreach ( var entityMapping in Model.EntityMappings )
            {
                // TJT: Better name?
                entityMapping.Compile();
            }

            // TODO: Model Validation...
        }

        #endregion
    }
}
