#region Namespaces

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Mapping
{
    public class EntityMapping<TEntity> : IEntityMapping where  TEntity : class
    {
        #region Fields

        private Dictionary<string, PropertyInfo> _propertyMap;
        private Dictionary<string, Func<TEntity, object>> Getters = new Dictionary<string, Func<TEntity, object>>();
        private Dictionary<string, Action<TEntity,object>> Setters = new Dictionary<string, Action<TEntity,object>>();

        #endregion

        #region Constructor(s)

        public EntityMapping()
        {
            InitializePropertyMappings();
        }

        #endregion

        #region Public Properties

        public object GetPropertyValue<T>( T entity, string propertyName ) where T : class => Getters[ propertyName ].Invoke( entity as TEntity );

        public void SetPropertyValue<T>( T entity, string propertyName, object value ) where T: class => Setters[ propertyName ].Invoke( entity as TEntity, value );

        public List<IPropertyMapping> PropertyMappings { get; set; } = new List<IPropertyMapping>();

        public List<IIndexMapping> IndexMappings { get; set; } = new List<IIndexMapping>();

        public List<IAssociationMapping> AssociationMappings { get; set; } = new List<IAssociationMapping>();

        public Type EntityType => typeof( TEntity );

        public string SchemaName { get; set; } = string.Empty;

        public string TableName { get; set; }

        public bool IsCaseSensitive { get; set; } = true;

        #endregion

        #region Public Methods
        
        public void Compile()
        {
            _propertyMap = PropertyMappings.ToDictionary( m => m.ColumnName, m => m.PropertyInfo, IsCaseSensitive? StringComparer.Ordinal : StringComparer.OrdinalIgnoreCase );

            CreateGetters();
            CreateSetters();
        }

        #endregion

        #region Private Methods

        private void CreateGetters()
        {
            foreach ( var propertyMapping in PropertyMappings )
            {
                PropertyInfo propertyInfo = propertyMapping.PropertyInfo;

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
            foreach ( var propertyMapping in PropertyMappings )
            {
                PropertyInfo propertyInfo = propertyMapping.PropertyInfo;
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

        private void InitializePropertyMappings()
        {
            // Set Table name
            TableName = (typeof( TEntity ).Name);

            // Set Properties
            bool hasKey = false;

            var entityProperties = EntityType.GetProperties().Where(
                p => p.CanRead && p.CanWrite &&
                     (p.GetMethod != null) && (p.SetMethod != null) &&
                     (p.GetMethod.IsPublic && p.SetMethod.IsPublic) &&
                     (!p.GetMethod.IsStatic) && (!p.SetMethod.IsStatic) ).ToList();

            foreach ( var propertyInfo in entityProperties )
            {
                if ( PropertyMappings.Any( p => p.PropertyName.Equals( propertyInfo.Name, StringComparison.InvariantCultureIgnoreCase ) ) )
                {
                    continue;
                }

                PropertyMapping propertyMapping = new PropertyMapping( propertyInfo );

                // Auto generate IsKey property for the entity
                if ( !hasKey )
                {
                    if ( string.Equals( propertyInfo.Name, "id", StringComparison.InvariantCultureIgnoreCase ) ||
                        string.Equals( propertyInfo.Name, TableName + "id", StringComparison.InvariantCultureIgnoreCase ) )
                    {
                        propertyMapping.IsKey = true;

                        hasKey = true;
                    }
                }

                //GuardForDuplicatePropertyMap( result );

                PropertyMappings.Add( propertyMapping );
            }
        }

        



        //public static IQueryable<dynamic> ToDynamic<T>( this IQueryable<T> query, ISet<String> fields )
        //{
        //    var pocoType = typeof( T );

        //    var itemParam = Expression.Parameter( pocoType, "x" );
        //    var members = fields.Select( f => Expression.PropertyOrField( itemParam, f ) );
        //    var addMethod = typeof( IDictionary<string, object> ).GetMethod(
        //                "Add", new Type[] { typeof( string ), typeof( object ) } );


        //    var elementInits = members.Select( m => Expression.ElementInit( addMethod, Expression.Constant( m.Member.Name ), Expression.Convert( m, typeof( Object ) ) ) );

        //    var expando = Expression.New( typeof( ExpandoObject ) );

        //    //var lambda = Expression.Lambda<Expression<Func<T, dynamic>>>( Expression.ListInit( expando, elementInits ), itemParam );
        //    var lambda = Expression.Lambda<Func<T, dynamic>>( Expression.ListInit( expando, elementInits ), itemParam );

        //    // query.Select( lambda.Compile() );
        //    return query.Select( lambda );
        //}

        //public static IQueryable<T> FromDynamic<T>( this IQueryable<dynamic> query ) where T : class, new()
        //{
        //    var itemParam = Expression.Parameter( typeof( ExpandoObject ), "x" );
        //    var members = typeof( T ).GetProperties().Where( p => p.CanWrite ).Select( f => Expression.Property( itemParam, f ) );
        //    var selector = Expression.MemberInit( Expression.New( typeof( T ) ),
        //        members.Select( m => Expression.Bind( typeof( T ).GetMember( m.Member.Name ).Single(), m ) )
        //        );
        //    var lambda = Expression.Lambda<Expression<Func<dynamic, T>>>( selector, itemParam );

        //    return query.Select( lambda.Compile() );
        //}

        //public static T FromDynamic<T>( this IDictionary<string, object> dictionary ) where T : class, new()
        //{
        //    var bindings = new List<MemberBinding>();
        //    foreach ( var sourceProperty in typeof( T ).GetProperties().Where( x => x.CanWrite ) )
        //    {
        //        var key = dictionary.Keys.SingleOrDefault( x => x.Equals( sourceProperty.Name, StringComparison.OrdinalIgnoreCase ) );

        //        if ( string.IsNullOrEmpty( key ) )
        //        {
        //            continue;
        //        }

        //        var propertyValue = dictionary[ key ];

        //        bindings.Add( Expression.Bind( sourceProperty, Expression.Constant( propertyValue ) ) );
        //    }

        //    Expression memberInit = Expression.MemberInit( Expression.New( typeof( T ) ), bindings );

        //    return Expression.Lambda<Func<T>>( memberInit ).Compile().Invoke();
        //}

        //public static dynamic ToDynamic<T>( this T obj, ISet<String> fields )
        //{
        //    IDictionary<string, object> expando = new ExpandoObject();

        //    var entityType = typeof( T );

        //    foreach ( var f in fields )
        //    {
        //        var propertyExpression = Expression.Property( Expression.Constant( obj ), entityType.GetProperty( f ) );
        //        var currentValue = Expression.Lambda<Func<string>>( propertyExpression ).Compile().Invoke();

        //        expando.Add( f, currentValue );
        //    }

        //    return expando as ExpandoObject;
        //}

        #endregion
    }
}
