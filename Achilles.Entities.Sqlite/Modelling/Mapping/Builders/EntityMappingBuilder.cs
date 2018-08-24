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

namespace Achilles.Entities.Modelling.Mapping.Builders
{
    /// <summary>
    /// TODO:
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityMappingBuilder<TEntity> : IEntityMappingBuilder<TEntity> where TEntity : class
    {
        #region Private Fields

        private EntityModel _model;

        private readonly List<ColumnMappingBuilder> _columnMappingBuilders = new List<ColumnMappingBuilder>();
        private readonly List<IndexMappingBuilder> _indexMappingBuilders = new List<IndexMappingBuilder>();
        private readonly List<HasManyMappingBuilder> _hasManyMappingBuilders = new List<HasManyMappingBuilder>();
        private readonly List<HasOneMappingBuilder<TEntity>> _hasOneMappingBuilders = new List<HasOneMappingBuilder<TEntity>>();

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Constructs a new EntityMappingBuilder instance. 
        /// </summary>
        public EntityMappingBuilder( EntityModel model )
        {
            _model = model;
            
            EntityMapping = model.GetOrAddEntityMapping( EntityType );
        }

        #endregion

        /// <inheritdoc/>
        public IEntityMapping EntityMapping { get; }

        /// <summary>
        /// 
        /// </summary>
        public Type EntityType => typeof( TEntity );

        /// <inheritdoc/>
        public IColumnMappingBuilder Column( Expression<Func<TEntity, object>> columnPropertyLambda )
        {
            var column = ReflectionHelper.GetMemberInfo( columnPropertyLambda );

            if ( _columnMappingBuilders.Any( builder => builder.Column == column ) )
            {
                throw new Exception( $"Duplicate mapping detected. Property '{column.Name}' is already mapped." );
            }
            
            // Will throw if column is not a property or field. 
            var columnMappingBuilder = new ColumnMappingBuilder( column );
            _columnMappingBuilders.Add( columnMappingBuilder );

            return columnMappingBuilder;
        }

        /// <inheritdoc/>
        public IIndexMappingBuilder HasIndex( Expression<Func<TEntity, object>> indexPropertyLambda )
        {
            var index = ReflectionHelper.GetMemberInfo( indexPropertyLambda );

            if ( _indexMappingBuilders.Any( builder => builder.IndexInfo == index ) )
            {
                throw new Exception( $"Duplicate mapping detected. Index property '{index.Name}' is already mapped." );
            }

            var indexMappingBuilder = new IndexMappingBuilder( index );
            _indexMappingBuilders.Add( indexMappingBuilder );

            return indexMappingBuilder;
        }

        /// <inheritdoc/>
        public IHasManyMappingBuilder HasMany( Expression<Func<TEntity, object>> relationshipPropertyLambda )
        {
            var relationship = ReflectionHelper.GetMemberInfo( relationshipPropertyLambda );

            if (  _hasManyMappingBuilders.Any( builder => builder.RelationshipProperty == relationship )  ||
                _hasOneMappingBuilders.Any( builder => builder.RelationshipProperty == relationship )  )
            {
                throw new Exception( $"Duplicate mapping detected. Relationship property '{relationship.Name}' is already mapped." );
            }

            var hasManyMappingBuilder = new HasManyMappingBuilder( relationship );
            _hasManyMappingBuilders.Add( hasManyMappingBuilder );

            return hasManyMappingBuilder;
        }

        /// <inheritdoc/>
        public IHasOneMappingBuilder<TEntity> HasOne( Expression<Func<TEntity, object>> relationshipPropertyLambda )
        {
            var relationship = ReflectionHelper.GetMemberInfo( relationshipPropertyLambda );

            if ( _hasManyMappingBuilders.Any( builder => builder.RelationshipProperty == relationship ) ||
                _hasOneMappingBuilders.Any( builder => builder.RelationshipProperty == relationship ) )
            {
                throw new Exception( $"Duplicate mapping detected. Relationship property '{relationship.Name}' is already mapped." );
            }

            var hasOneMappingBuilder = new HasOneMappingBuilder<TEntity>( relationship );
            _hasOneMappingBuilders.Add( hasOneMappingBuilder );

            return hasOneMappingBuilder;
        }

        /// <inheritdoc/>
        public void IsCaseSensitive( bool caseSensitive ) => EntityMapping.IsCaseSensitive = caseSensitive;

        /// <inheritdoc/>
        public void ToTable( string tableName ) => EntityMapping.TableName = tableName;

        /// <inheritdoc/>
        public IEntityMapping Build()
        {
            var columnMappings = _columnMappingBuilders.Select( b => b.Build() ).ToList();

            // Add or update the column in the EntityMapping
            foreach ( var columnMapping in columnMappings )
            {
                var propertyMappingIndex = EntityMapping.ColumnMappings.FindIndex( p => p.PropertyName == columnMapping.PropertyName );

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

            // Add foreign key mappings...

            // HasMany mappings may have the foreign key constaint on another Entity ( unless self referencing )
            var hasManyMappings = _hasManyMappingBuilders.Select( b => b.Build( EntityMapping ) ).ToList();

            foreach ( var hasManyMapping in hasManyMappings )
            {
                var t = hasManyMapping.ForeignKeyMapping.ForeignKeyProperty.DeclaringType;

                if ( t != EntityType )
                {
                    var foreignKeyConstraintMapping = _model.GetOrAddEntityMapping( t );
                    foreignKeyConstraintMapping.ForeignKeyMappings.Add( hasManyMapping.ForeignKeyMapping );
                }
                else
                {
                    EntityMapping.ForeignKeyMappings.Add( hasManyMapping.ForeignKeyMapping );
                }

                EntityMapping.RelationshipMappings.Add( hasManyMapping );
            }

            var hasOneMappings = _hasOneMappingBuilders.Select( b => b.Build( EntityMapping ) ).ToList();

            foreach ( var hasOneMapping in hasOneMappings )
            {
                EntityMapping.RelationshipMappings.Add( hasOneMapping );
                EntityMapping.ForeignKeyMappings.Add( hasOneMapping.ForeignKeyMapping );
            }

            return EntityMapping;
        }
    }
}
