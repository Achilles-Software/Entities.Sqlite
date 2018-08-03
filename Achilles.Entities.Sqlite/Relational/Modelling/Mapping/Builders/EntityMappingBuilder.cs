#region Namespaces

using Achilles.Entities.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Mapping.Builders
{
    public class EntityMappingBuilder<TEntity> : IEntityMappingBuilder<TEntity> where TEntity : class
    {
        private readonly List<IPropertyMappingBuilder> _propertyMappingBuilders = new List<IPropertyMappingBuilder>();
        private readonly List<IIndexMappingBuilder> _indexMappingBuilders = new List<IIndexMappingBuilder>();
        private readonly List<IAssociationMappingBuilder> _relationshipMappingBuilders = new List<IAssociationMappingBuilder>();

        public EntityMappingBuilder()
        {
            EntityMapping = CreateEntityMapping();
        }

        protected virtual IEntityMapping CreateEntityMapping() => new EntityMapping<TEntity>();

        protected virtual IPropertyMappingBuilder CreatePropertyMappingBuilder( PropertyInfo propertyInfo ) => new PropertyMappingBuilder( propertyInfo );

        public IEntityMapping EntityMapping { get; }

        public IPropertyMappingBuilder Property( Expression<Func<TEntity, object>> mapping )
        {
            // Resolve property info from expression and guard against duplicate mappings.
            var propertyInfo = (PropertyInfo)ReflectionHelper.GetMemberInfo( mapping );

            if ( _propertyMappingBuilders.Any( builder => builder.Property == propertyInfo ) )
            {
                throw new Exception( $"Duplicate mapping detected. Property '{propertyInfo.Name}' is already mapped." );
            }

            // Create a mapping builder from the property info and assign it to the entity mapping.
            var propertyMappingBuilder = CreatePropertyMappingBuilder( propertyInfo );
            _propertyMappingBuilders.Add( propertyMappingBuilder );

            // Return mapping builder for chaining
            return propertyMappingBuilder;
        }

        public IIndexMappingBuilder Index( Expression<Func<TEntity, object>> mapping )
        {
            // Resolve property info from expression and guard against duplicate mappings.
            var propertyInfo = (PropertyInfo)ReflectionHelper.GetMemberInfo( mapping );

            if ( _indexMappingBuilders.Any( builder => builder.Property == propertyInfo ) )
            {
                throw new Exception( $"Duplicate index mapping detected. Property '{propertyInfo.Name}' is already indexed." );
            }

            var indexMappingBuilder = new IndexMappingBuilder( propertyInfo );
            _indexMappingBuilders.Add( indexMappingBuilder );

            return indexMappingBuilder;
        }

        public IAssociationMappingBuilder Relationship( Expression<Func<TEntity, object>> mapping )
        {
            // FIXME:

            // Resolve property info from expression and guard against duplicate mappings.
            //var propertyInfo = (PropertyInfo)ReflectionHelper.GetMemberInfo( mapping );

            //if ( _relationshipMappingBuilders.Any( builder => builder.Property == propertyInfo ) )
            //{
            //    throw new Exception( $"Duplicate mapping detected. Property '{propertyInfo.Name}' is already mapped." );
            //}

            var relationshipMappingBuilder = new RelationshipMappingBuilder();
            _relationshipMappingBuilders.Add( relationshipMappingBuilder );

            return relationshipMappingBuilder;
        }

        public void IsCaseSensitive( bool caseSensitive ) => EntityMapping.IsCaseSensitive = caseSensitive;

        public void ToTable( string tableName ) => EntityMapping.TableName = tableName;

        public IEntityMapping Build()
        {
            var propertyMaps = _propertyMappingBuilders.Select( b => b.Build() ).ToList();

            // Add or update the properties in the EntityMapping
            foreach ( var propertyMap in propertyMaps )
            {
                var propertyMappingIndex = EntityMapping.PropertyMappings.FindIndex( p => p.PropertyName == propertyMap.PropertyName );

                if ( propertyMappingIndex >= 0 )
                {
                    EntityMapping.PropertyMappings[ propertyMappingIndex ] = propertyMap;
                }
                else
                {
                    EntityMapping.PropertyMappings.Add( propertyMap );
                }
            }

            // Add index mappings
            var indexMappings = _indexMappingBuilders.Select( b => b.Build() ).ToList();
            EntityMapping.IndexMappings.AddRange( indexMappings );

            // Add relatinship mappings
            // TODO:
            //var relationshipMappings = _relationshipMappingBuilders.Select( b => b.Build() ).ToList();
            //EntityMapping.RelationshipMappings.AddRange( relationshipMappings );

            return EntityMapping;
        }
    }
}
