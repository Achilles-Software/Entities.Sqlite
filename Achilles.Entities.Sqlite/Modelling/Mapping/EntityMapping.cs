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
            _columnMapping = ColumnMappings.ToDictionary( m => m.ColumnName, m => m.MemberInfo, IsCaseSensitive? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase );

            CreateGetters();
            CreateSetters();
        }

        #endregion

        #region Private Methods

        private void CreateGetters()
        {
            foreach ( var columnMapping in ColumnMappings )
            {
                PropertyInfo propertyInfo = columnMapping.MemberInfo as PropertyInfo;

                ParameterExpression instance = Expression.Parameter( typeof( TEntity ), "instance" );

                var body = Expression.Call( instance, propertyInfo.GetGetMethod() );
                var parameters = new ParameterExpression[] { instance };
                Expression conversion = Expression.Convert( body, typeof( object ) );

                var getter = Expression.Lambda<Func<TEntity, object>>( conversion, parameters ).Compile();
                
                Getters.Add( propertyInfo.Name, getter );
            }
        }

        private void CreateSetters()
        {
            foreach ( var columnMapping in ColumnMappings )
            {
                PropertyInfo propertyInfo = columnMapping.MemberInfo as PropertyInfo;
                var setMethod = propertyInfo.GetSetMethod();
                var setMethodParameterType = setMethod.GetParameters().First().ParameterType;

                ParameterExpression instance = Expression.Parameter( typeof( TEntity ), "instance" );
                var parameterExpression = Expression.Parameter( typeof( object ), "value" );
                Expression conversion = Expression.Convert( parameterExpression, setMethodParameterType );

                var body = Expression.Call( instance, setMethod, conversion );
                var parameters = new ParameterExpression[] { instance, parameterExpression };

                var setter = Expression.Lambda<Action<TEntity,object>>( body, parameters ).Compile();

                Setters.Add( propertyInfo.Name, setter );
            }
        }

        private void InitializePropertyAndFieldMappings()
        {
            // TODO: GetFields()...

            TableName = (typeof( TEntity ).Name);

            bool hasKey = false;

            var entityProperties = EntityType.GetProperties().Where(
                p => p.CanRead && p.CanWrite &&
                     (p.GetMethod != null) && (p.SetMethod != null) &&
                     (p.GetMethod.IsPublic && p.SetMethod.IsPublic) &&
                     (!p.GetMethod.IsStatic) && (!p.SetMethod.IsStatic) ).ToList();

            foreach ( var propertyInfo in entityProperties )
            {
                if ( !propertyInfo.PropertyType.IsScalarType() )
                {
                    // Non scalar types must be mapped in OnModelBuilding()
                    continue;
                }

                if ( ColumnMappings.Any( p => p.MemberName.Equals( propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase ) ) )
                {
                    continue;
                }

                ColumnMapping columnMapping = new ColumnMapping( propertyInfo, isProperty: true );

                // Auto generate IsKey property for the entity
                if ( !hasKey )
                {
                    if ( string.Equals( propertyInfo.Name, "id", StringComparison.InvariantCultureIgnoreCase ) ||
                        string.Equals( propertyInfo.Name, TableName + "id", StringComparison.InvariantCultureIgnoreCase ) )
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
