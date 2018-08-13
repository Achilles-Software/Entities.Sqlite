#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Reflection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Relational.Modelling.Mapping.Builders
{
    public class EntityMappingBuilder<TEntity> : IEntityMappingBuilder<TEntity> where TEntity : class
    {
        private readonly List<IColumnMappingBuilder> _columnMappingBuilders = new List<IColumnMappingBuilder>();
        private readonly List<IIndexMappingBuilder> _indexMappingBuilders = new List<IIndexMappingBuilder>();
        private readonly List<IHasManyMappingBuilder> _hasManyMappingBuilders = new List<IHasManyMappingBuilder>();
        private readonly List<IHasOneMappingBuilder<TEntity>> _hasOneMappingBuilders = new List<IHasOneMappingBuilder<TEntity>>();

        public EntityMappingBuilder()
        {
            EntityMapping = CreateEntityMapping();
        }

        protected virtual IEntityMapping CreateEntityMapping() => new EntityMapping<TEntity>();

        protected virtual IColumnMappingBuilder CreateColumnMappingBuilder( MemberInfo propertyOrFieldInfo ) => new ColumnMappingBuilder( propertyOrFieldInfo );

        public IEntityMapping EntityMapping { get; }

        public IColumnMappingBuilder Column( Expression<Func<TEntity, object>> columnMapping )
        {
            // Resolve property info from expression and guard against duplicate mappings.
            var memberInfo = ReflectionHelper.GetMemberInfo( columnMapping );

            //var propertyInfo = (PropertyInfo)ReflectionHelper.GetMemberInfo( columnMapping );

            //if ( _columnMappingBuilders.Any( builder => builder.MemberInfo == propertyInfo ) )
            //{
            //    throw new Exception( $"Duplicate mapping detected. Property '{propertyInfo.Name}' is already mapped." );
            //}

            // Create a mapping builder from the property info and assign it to the entity mapping.
            var columnMappingBuilder = CreateColumnMappingBuilder( memberInfo );
            _columnMappingBuilders.Add( columnMappingBuilder );

            // Return mapping builder for chaining
            return columnMappingBuilder;
        }

        public IIndexMappingBuilder HasIndex( Expression<Func<TEntity, object>> mapping )
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

        public IHasManyMappingBuilder HasMany( Expression<Func<TEntity, object>> mapping )
        {
            // TODO:

            // Resolve property info from expression and guard against duplicate mappings.
            //var propertyInfo = (PropertyInfo)ReflectionHelper.GetMemberInfo( mapping );

            //if ( _relationshipMappingBuilders.Any( builder => builder.Property == propertyInfo ) )
            //{
            //    throw new Exception( $"Duplicate mapping detected. Property '{propertyInfo.Name}' is already mapped." );
            //}

            var hasManyRelationshipMappingBuilder = new HasManyMappingBuilder();
            _hasManyMappingBuilders.Add( hasManyRelationshipMappingBuilder );

            return hasManyRelationshipMappingBuilder;
        }

        public IHasOneMappingBuilder<TEntity> HasOne( Expression<Func<TEntity, object>> mapping )
        {
            // FIXME:

            // Resolve property info from expression and guard against duplicate mappings.
            //var propertyInfo = (PropertyInfo)ReflectionHelper.GetMemberInfo( mapping );

            //if ( _relationshipMappingBuilders.Any( builder => builder.Property == propertyInfo ) )
            //{
            //    throw new Exception( $"Duplicate mapping detected. Property '{propertyInfo.Name}' is already mapped." );
            //}

            var hasOneRelationshipMappingBuilder = new HasOneMappingBuilder<TEntity>();
            _hasOneMappingBuilders.Add( hasOneRelationshipMappingBuilder );

            return hasOneRelationshipMappingBuilder;
        }

        public void IsCaseSensitive( bool caseSensitive ) => EntityMapping.IsCaseSensitive = caseSensitive;

        public void ToTable( string tableName ) => EntityMapping.TableName = tableName;

        public IEntityMapping Build()
        {
            var columnMappings = _columnMappingBuilders.Select( b => b.Build() ).ToList();

            // Add or update the column in the EntityMapping
            foreach ( var columnMapping in columnMappings )
            {
                var propertyMappingIndex = EntityMapping.ColumnMappings.FindIndex( p => p.MemberName == columnMapping.MemberName );

                if ( propertyMappingIndex >= 0 )
                {
                    EntityMapping.ColumnMappings[ propertyMappingIndex ] = columnMapping;
                }
                else
                {
                    EntityMapping.ColumnMappings.Add( columnMapping );
                }
            }

            // Add index mappings
            var indexMappings = _indexMappingBuilders.Select( b => b.Build() ).ToList();
            EntityMapping.IndexMappings.AddRange( indexMappings );

            // Add association mappings
            var hasManyMappings = _hasManyMappingBuilders.Select( b => b.Build() ).ToList();
            EntityMapping.ForeignKeyMappings.AddRange( hasManyMappings );

            return EntityMapping;
        }
    }
}
