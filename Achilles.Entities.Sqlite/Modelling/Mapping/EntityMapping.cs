#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

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

        private Dictionary<string, MemberInfo> _columnMapping;

        private Dictionary<string, Func<TEntity, object>> Getters = new Dictionary<string, Func<TEntity, object>>();
        private Dictionary<string, Action<TEntity,object>> Setters = new Dictionary<string, Action<TEntity,object>>();

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Constructs a new instance of <see cref="EntityMapping"/>.
        /// </summary>
        public EntityMapping()
        {
            InitializePropertyAndFieldMappings();
        }

        #endregion

        #region Public Properties

        public object GetPropertyValue<T>( T entity, string propertyName ) where T : class => Getters[ propertyName ].Invoke( entity as TEntity );

        public void SetPropertyValue<T>( T entity, string propertyName, object value ) where T: class => Setters[ propertyName ].Invoke( entity as TEntity, value );

        public List<IColumnMapping> ColumnMappings { get; set; } = new List<IColumnMapping>();

        public List<IIndexMapping> IndexMappings { get; set; } = new List<IIndexMapping>();

        public List<IForeignKeyMapping> ForeignKeyMappings { get; set; } = new List<IForeignKeyMapping>();

        public Type EntityType => typeof( TEntity );

        public string SchemaName { get; set; } = string.Empty;

        public string TableName { get; set; }

        public bool IsCaseSensitive { get; set; } = true;

        #endregion

        #region Public Methods
        
        public void Compile()
        {
            _columnMapping = ColumnMappings.ToDictionary( m => m.ColumnName, m => m.ColumnInfo, IsCaseSensitive? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase );

            CreateGetters();
            CreateSetters();
        }

        #endregion

        #region Private Methods

        private void CreateGetters()
        {
            foreach ( var columnMapping in ColumnMappings )
            {
                if ( columnMapping.ColumnInfo is PropertyInfo propertyInfo )
                {
                    ParameterExpression instance = Expression.Parameter( typeof( TEntity ), "instance" );

                    var body = Expression.Call( instance, propertyInfo.GetGetMethod() );
                    var parameters = new ParameterExpression[] { instance };
                    Expression conversion = Expression.Convert( body, typeof( object ) );

                    var getter = Expression.Lambda<Func<TEntity, object>>( conversion, parameters ).Compile();

                    Getters.Add( propertyInfo.Name, getter );
                }
                else if ( columnMapping.ColumnInfo is FieldInfo field )
                {
                    ParameterExpression instance = Expression.Parameter( typeof( TEntity ), "instance" );

                    MemberExpression fieldExpression = Expression.Field( instance, field );
                    var parameters = new ParameterExpression[] { instance };
                    Expression conversion = Expression.Convert( fieldExpression, typeof( object ) );

                    var getter = Expression.Lambda<Func<TEntity, object>>( conversion, parameters ).Compile();

                    Getters.Add( field.Name, getter );
                }
            }
        }

        private void CreateSetters()
        {
            foreach ( var columnMapping in ColumnMappings )
            {
                if ( columnMapping.ColumnInfo is PropertyInfo propertyInfo )
                {
                    var setMethod = propertyInfo.GetSetMethod();
                    var setMethodParameterType = setMethod.GetParameters().First().ParameterType;

                    ParameterExpression instance = Expression.Parameter( typeof( TEntity ), "instance" );
                    var parameterExpression = Expression.Parameter( typeof( object ), "value" );
                    Expression conversion = Expression.Convert( parameterExpression, setMethodParameterType );

                    var body = Expression.Call( instance, setMethod, conversion );
                    var parameters = new ParameterExpression[] { instance, parameterExpression };

                    var setter = Expression.Lambda<Action<TEntity, object>>( body, parameters ).Compile();

                    Setters.Add( propertyInfo.Name, setter );
                }
                else if ( columnMapping.ColumnInfo is FieldInfo field )
                {
                    ParameterExpression instance = Expression.Parameter( typeof( TEntity ), "instance" );
                    ParameterExpression valueExpression = Expression.Parameter( typeof( object ), "value" );
                    Expression conversion = Expression.Convert( valueExpression, field.FieldType );

                    MemberExpression fieldExpression = Expression.Field( instance, field );
                    BinaryExpression assignExp = Expression.Assign( fieldExpression, conversion );

                    var setter = Expression.Lambda<Action<TEntity, object>>
                        ( assignExp, instance, valueExpression ).Compile();

                    Setters.Add( field.Name, setter );
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
