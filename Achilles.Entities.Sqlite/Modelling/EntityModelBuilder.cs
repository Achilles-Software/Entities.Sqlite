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
    /// <see cref="IEntityModelBuilder"/> implementation of the fluent builder inteface for relational database modelling.
    /// </summary>
    public class EntityModelBuilder : IEntityModelBuilder
    {
        #region Constructor(s)

        /// <summary>
        /// Constructs a new instance of <see cref="EntityModelBuilder"/>.
        /// </summary>
        public EntityModelBuilder()
        {
            EntityMappings = new EntityMappingCollection();
        }

        #endregion

        #region Public Properties

        /// <inheritdoc/>
        private EntityMappingCollection EntityMappings { get; }

        #endregion

        #region Public Methods

        /// <inheritdoc/>
        public void Entity<TEntity>( Action<IEntityMappingBuilder<TEntity>> action ) where TEntity : class
        {
            var builder = new EntityMappingBuilder<TEntity>( EntityMappings );

            action( builder );

            AddEntityMapping( builder );
        }

        /// <inheritdoc />
        public IEntityModel Build( DataContext context )
        {
            // TJT: Should all entities be added in the user's data context constructor?
            // What should happen if an entity is referenced during model building that is
            // not in the context?

            foreach ( var entitySet in context.EntitySets )
            {
                EntityMappings.GetOrAddEntityMapping( entitySet.EntityType );
            }

            context.OnModelBuilding( this );

            // TODO: Model Validation...

            // The model entity mappings have now been configured.
            // Resolve Entity references and validate the model. 

            ResolveMappingsAndValidateModel();

            return new EntityModel( EntityMappings );
        }

        #endregion

        #region Private Methods

        private void AddEntityMapping<TEntity>( EntityMappingBuilder<TEntity> entityMappingBuilder ) where TEntity : class
        {
            var EntityMapping = entityMappingBuilder.Build();

            EntityMappings.TryAddEntityMapping( typeof( TEntity ), EntityMapping );
        }

        private void ResolveMappingsAndValidateModel()
        {

        }

        #endregion
    }
}
