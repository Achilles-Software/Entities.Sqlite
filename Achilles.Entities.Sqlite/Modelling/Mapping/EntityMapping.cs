#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Modelling.Mapping.Accessors;
using Achilles.Entities.Relational.Modelling.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public class EntityMapping<TEntity> : IEntityMapping where  TEntity : class
    {
        #region Fields

        private EntityModel _model;

        private Dictionary<string, MemberInfo> _columnProperties;
        private Dictionary<string, MemberInfo> _foreignKeyProperties;
        private Dictionary<string, MemberInfo> _relationshipProperties;

        private Dictionary<string, MemberAccessor<TEntity,object>> ColumnAccessors 
            = new Dictionary<string, MemberAccessor<TEntity,object>>();

        private Dictionary<string, MemberAccessor<TEntity,object>> ForeignKeyAccessors 
            = new Dictionary<string, MemberAccessor<TEntity, object>>();

        private Dictionary<string, MemberAccessor<TEntity, IEnumerable<TEntity>>> EntityReferenceAccessors 
            = new Dictionary<string, MemberAccessor<TEntity, IEnumerable<TEntity>>>();

        private Dictionary<string, MemberAccessor<TEntity, IEnumerable<TEntity>>> EntityCollectionAccessors 
            = new Dictionary<string, MemberAccessor<TEntity, IEnumerable<TEntity>>>();
        
        #endregion

        #region Constructor(s)

        /// <summary>
        /// Constructs a new instance of <see cref="EntityMapping{TEntity}"/>.
        /// </summary>
        public EntityMapping( EntityModel model )
        {
            _model = model;

            InitializePropertyAndFieldMappings();
        }

        #endregion

        #region Public Properties

        public object GetColumn<T>( T entity, string propertyName ) where T : class 
            => ColumnAccessors[ propertyName ].GetValue( entity as TEntity );

        public void SetColumn<T>( T entity, string propertyName, object value ) where T: class 
            => ColumnAccessors[ propertyName ].SetValue( entity as TEntity, value );

        public void SetEntityReference<T>( T Entity, string propertyName, object source ) where T : class
            => EntityReferenceAccessors[ propertyName ].SetValue( Entity as TEntity, source );     

        public List<IColumnMapping> ColumnMappings { get; set; } = new List<IColumnMapping>();

        public List<IIndexMapping> IndexMappings { get; set; } = new List<IIndexMapping>();

        public List<IForeignKeyMapping> ForeignKeyMappings { get; set; } = new List<IForeignKeyMapping>();

        public List<IRelationshipMapping> RelationshipMappings { get; set; } = new List<IRelationshipMapping>();

        public Type EntityType => typeof( TEntity );

        public string SchemaName { get; set; } = string.Empty;

        public string TableName { get; set; }

        public bool IsCaseSensitive { get; set; } = true;

        #endregion

        #region Internal Properties

        internal EntitySet<TEntity> EntitySet => (EntitySet< TEntity>)_model.DataContext.EntitySets[ EntityType ];

        #endregion

        #region Public Methods

        public void Compile()
        {
            _columnProperties = ColumnMappings.ToDictionary( m => m.ColumnName, m => m.ColumnInfo, IsCaseSensitive? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase );

            CreateAccessors();
        }

        #endregion



        #region Private Methods

        private void CreateAccessors()
        {
            // Columns...
            foreach ( var columnMapping in ColumnMappings )
            {
                ColumnAccessors.Add( columnMapping.PropertyName, new ColumnAccessor<TEntity,object>( columnMapping.ColumnInfo ) );
            }

            // Foreign Keys...
            foreach ( var foreignKeyMapping in ForeignKeyMappings )
            {
                ForeignKeyAccessors.Add( foreignKeyMapping.PropertyName, new ForeignKeyAccessor<TEntity, object>( foreignKeyMapping.ForeignKeyProperty ) );
            }

            // Relationship Mappings...
            foreach ( var relationshipMapping in RelationshipMappings )
            {
                if ( relationshipMapping.IsMany )
                {
                    EntityCollectionAccessors.Add(
                        relationshipMapping.RelationshipProperty.Name,
                        new EntityCollectionAccessor<TEntity, IEnumerable<TEntity>>( relationshipMapping.RelationshipProperty ) );
                }
                else
                {
                    EntityReferenceAccessors.Add(
                        relationshipMapping.RelationshipProperty.Name,
                        new EntityReferenceAccessor<TEntity, IEnumerable<TEntity>>( relationshipMapping.RelationshipProperty ) );

                }
            }
        }

        /// <summary>
        /// Create column mappings for entity properties and fields.
        /// </summary>
        private void InitializePropertyAndFieldMappings()
        {
            TableName = typeof( TEntity ).Name;
            bool hasKey = false;

            var entityMembers = EntityType.GetMembers()
                .Where( m => m.MemberType == MemberTypes.Field || m.MemberType == MemberTypes.Property ).ToList();

            foreach ( var member in entityMembers )
            {
                ColumnMapping columnMapping;

                if ( ColumnMappings.Any( p => p.PropertyName.Equals( member.Name, StringComparison.InvariantCultureIgnoreCase ) ) )
                {
                    // Already mapped.
                    continue;
                }

                if ( member is PropertyInfo property && property.CanRead && property.CanWrite &&
                    (property.GetMethod != null) && (property.SetMethod != null) &&
                    (property.GetMethod.IsPublic && property.SetMethod.IsPublic) &&
                    (!property.GetMethod.IsStatic) && (!property.SetMethod.IsStatic) )
                {
                    if ( !property.PropertyType.IsScalarType() )
                    {
                        // Non-scalar types ( relational types: TEntity or IEnumerable<TEntity> ) currently must be mapped in OnModelBuilding()
                        // TODO: Add 1-1 mapping convention and 1-many mapping convention.
                        continue;
                    }

                    columnMapping = new ColumnMapping( property );
                }
                else if ( member is FieldInfo field && field.IsPublic && !field.IsStatic )
                {
                    if ( !field.FieldType.IsScalarType() )
                    {
                        // Non-scalar types ( relational types: TEntity or IEnumerable<TEntity> ) currently must be mapped in OnModelBuilding()
                        // TODO: Add 1-1 mapping convention and 1-many mapping convention.
                        continue;
                    }

                    columnMapping = new ColumnMapping( field );
                }
                else
                {
                    // Not an acceptable field or property.
                    // TJT: Throw here?
                    continue;
                }

                // Auto generate IsKey property for the entity
                if ( !hasKey )
                {
                    if ( string.Equals( member.Name, "id", StringComparison.InvariantCultureIgnoreCase ) ||
                        string.Equals( member.Name, TableName + "id", StringComparison.InvariantCultureIgnoreCase ) )
                    {
                        columnMapping.IsKey = true;

                        hasKey = true;
                    }
                }

                ColumnMappings.Add( columnMapping );
            }
        }

        #endregion
    }
}
