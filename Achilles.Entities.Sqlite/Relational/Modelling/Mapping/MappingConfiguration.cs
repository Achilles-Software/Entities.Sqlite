#region Namespaces

using Achilles.Entities.Mapping;
using Achilles.Entities.Mapping.Builders;
using System;

#endregion

namespace Achilles.Entities
{
    /// <summary>
    /// <see cref="IMappingConfiguration"/> implementation for fluent mapping configuration.
    /// </summary>
    public class MappingConfiguration : IMappingConfiguration
    {
        #region Constructor(s)

        public MappingConfiguration()
        {
            EntityMappings = new EntityMappingCollection();
        }

        #endregion

        #region Public Properties

        public EntityMappingCollection EntityMappings { get; }

        #endregion

        #region Public Methods

        public void AddMap<TEntity>( IEntityMappingBuilder<TEntity> EntityMappingBuilder )
        {
            var EntityMapping = EntityMappingBuilder.Build();

            EntityMappings.Add( typeof( TEntity ), EntityMapping );
        }

        public void Entity<TEntity>( Action<IEntityMappingBuilder<TEntity>> action ) where TEntity : class
        {
            var builder = new EntityMappingBuilder<TEntity>();

            action( builder );
            AddMap( builder );
        }

        #endregion
    }
}
