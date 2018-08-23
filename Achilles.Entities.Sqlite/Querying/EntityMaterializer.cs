#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using Achilles.Entities.Extensions;
using Achilles.Entities.Modelling;
using Achilles.Entities.Querying.TypeConverters;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

#endregion

namespace Achilles.Entities.Querying
{
    internal class EntityMaterializer
    {
        #region Fields

        private static readonly ConcurrentDictionary<Type, EntityTypeMap> TypeMapCache = new ConcurrentDictionary<Type, EntityTypeMap>();

        private static Dictionary<InstanceKey, object> _instanceCache;

        private static readonly List<ITypeConverter> TypeConverters = new List<ITypeConverter>();

        private DataContext _context;

        private IEntityModel _model => _context.Model;

        #endregion

        #region Constructor(s)

        internal EntityMaterializer( DataContext context )
        {
            _context = context;

            Initialize();
        }

        #endregion

        #region API Methods

        /// <summary>
        /// Converts a list of dictionaries of property names and values to a list of type of <typeparamref name="TEntity"/>.
        /// 
        /// Population of complex nested child properties is supported by underscoring "_" into the
        /// nested child properties in the property name.
        /// </summary>
        /// <typeparam name="TEntity">Type to instantiate and automap to</typeparam>
        /// <param name="listOfProperties">List of property names and values</param>
        /// <param name="keepCache">If false, clears instance cache after mapping is completed. Defaults to true, meaning instances are kept between calls.</param>
        /// <returns>List of type <typeparamref name="TEntity"/></returns>
        public IEnumerable<TEntity> Materialize<TEntity>( IEnumerable<IDictionary<string, object>> listOfProperties, bool keepCache = true )
        {
            return Materialize( typeof( TEntity ), listOfProperties, keepCache ).Cast<TEntity>();
        }

        /// <summary>
        /// Converts a list of dictionaries of property names and values to a list of specified Type.
        /// 
        /// Population of complex nested child properties is supported by underscoring "_" into the
        /// nested child properties in the property name.
        /// </summary>
        /// <param name="type">Type to instantiate and automap to</param>
        /// <param name="listOfProperties">List of property names and values</param>
        /// <param name="keepCache">If false, clears instance cache after mapping is completed. Defaults to true, meaning instances are kept between calls.</param>
        /// <returns>List of specified Type</returns>
        public IEnumerable<object> Materialize( Type type, IEnumerable<IDictionary<string, object>> listOfProperties, bool keepCache = true )
        {
            var instanceCache = new Dictionary<object, object>();

            foreach ( var properties in listOfProperties )
            {
                var getInstanceResult = GetInstance( type, properties );

                object instance = getInstanceResult.Item2;

                var key = getInstanceResult.Item3;

                if ( instanceCache.ContainsKey( key ) == false )
                {
                    instanceCache.Add( key, instance );
                }

                var caseInsensitiveDictionary = new Dictionary<string, object>( properties, StringComparer.OrdinalIgnoreCase );

                Materialize( caseInsensitiveDictionary, instance );
            }

            if ( !keepCache )
            {
                ClearInstanceCache();
            }

            return instanceCache.Select( pair => pair.Value );
        }

        #endregion

        #region Private Materialization

        private void Initialize()
        {
            //ApplyDefaultIdentifierConventions();
            ApplyDefaultTypeConverters();
        }

        private void ApplyDefaultTypeConverters()
        {
            TypeConverters.Add( new GuidConverter() );
            TypeConverters.Add( new EnumConverter() );
            TypeConverters.Add( new ValueTypeConverter() );
        }

        /// <summary>
        /// Populates the given instance's properties where the IDictionary key property names
        /// match the type's property names case insensitively.
        /// 
        /// Population of complex nested child properties is supported by underscoring "_" into the
        /// nested child properties in the property name.
        /// </summary>
        /// <param name="dictionary">Dictionary of property names and values</param>
        /// <param name="entity">Instance to populate</param>
        /// <param name="parentInstance">Optional parent instance of the instance being populated</param>
        /// <returns>Populated instance</returns>
        internal object Materialize( IDictionary<string, object> dictionary, object entity, object parentInstance = null )
        {
            if ( entity.GetType().IsPrimitive || entity is string )
            {
                object value;
                if ( !dictionary.TryGetValue( "$", out value ) )
                {
                    throw new InvalidCastException( "For lists of primitive types, include $ as the name of the property" );
                }

                entity = value;

                return entity;
            }

            // Once we have the instance we can attach an entity set source to the instance relationship properties
            var entityMapping = _model.GetEntityMapping( entity.GetType() );

            var relationshipMappings = entityMapping.RelationshipMappings;

            var fieldsAndProperties = GetFieldsAndProperties( entity.GetType() );

            foreach ( var fieldOrProperty in fieldsAndProperties )
            {
                var memberName = fieldOrProperty.Key.ToLower();
                var memberInfo = fieldOrProperty.Value;

                var relationshipMapping = relationshipMappings.Where( r => r.RelationshipProperty == memberInfo ).FirstOrDefault();

                if ( relationshipMapping != null )
                {
                    if ( relationshipMapping.IsMany )
                    {
                        IEntityCollection entityCollection = memberInfo as IEntityCollection;
                    }
                    else
                    {
                        // First Get the type entity we are materializing
                        Type entityType = entity.GetType();

                        object entityReferenceProperty;

                        if ( memberInfo.MemberType == MemberTypes.Property )
                        {
                            PropertyInfo propertyInfo = memberInfo as PropertyInfo;

                            entityReferenceProperty = propertyInfo.GetValue( entity );
                        }
                        else
                        {
                            FieldInfo fieldInfo = memberInfo as FieldInfo;

                            entityReferenceProperty = fieldInfo.GetValue( entity );
                        }

                        Type entityReferencePropertyType = entityReferenceProperty.GetType();
                        MethodInfo methodOfMainProperty = entityReferencePropertyType.GetMethod( "AttachSource" );

                        // Get the EntitySet<TEntity>
                        var entityReference = entityReferencePropertyType.GetGenericArguments().First();
                        // Get the EntitySet from the foreign key type
                        var entitySet = _context.EntitySets[ entityReference ];

                        methodOfMainProperty.Invoke( entityReferenceProperty, new object[] { entitySet } );

                    }

                    continue;
                }

                object value;

                // Handle populating simple members on the current type
                if ( dictionary.TryGetValue( memberName, out value ) )
                {
                    SetMemberValue( memberInfo, entity, value );
                }
                else
                {
                    Type memberType = GetMemberType( memberInfo );

                    // Handle populating complex members on the current type
                    if ( memberType.IsClass || memberType.IsInterface )
                    {
                        // Try to find any keys that start with the current member name
                        var nestedDictionary = dictionary.Where( x => x.Key.ToLower().StartsWith( memberName + "_" ) ).ToList();

                        // If there weren't any keys
                        if ( !nestedDictionary.Any() )
                        {
                            // And the parent instance was not null
                            if ( parentInstance != null )
                            {
                                // And the parent instance is of the same type as the current member
                                if ( parentInstance.GetType() == memberType )
                                {
                                    // Then this must be a 'parent' to the current type
                                    SetMemberValue( memberInfo, entity, parentInstance );
                                }
                            }

                            continue;
                        }
                        var regex = new Regex( Regex.Escape( memberName + "_" ) );
                        var newDictionary = nestedDictionary.ToDictionary( pair => regex.Replace( pair.Key.ToLower(), string.Empty, 1 ),
                            pair => pair.Value, StringComparer.OrdinalIgnoreCase );

                        // Try to get the value of the complex member. If the member
                        // hasn't been initialized, then this will return null.
                        object nestedInstance = GetMemberValue( memberInfo, entity );

                        var genericCollectionType = typeof( IEnumerable<> );
                        var isEnumerableType = memberType.IsGenericType && genericCollectionType.IsAssignableFrom( memberType.GetGenericTypeDefinition() )
                                               || memberType.GetInterfaces().Any( x => x.IsGenericType && x.GetGenericTypeDefinition() == genericCollectionType );

                        // If the member is null and is a class or interface (not ienumerable), try to create an instance of the type
                        if ( nestedInstance == null && (memberType.IsClass || (memberType.IsInterface && !isEnumerableType)) )
                        {
                            if ( memberType.IsArray )
                            {
                                nestedInstance = new ArrayList().ToArray( memberType.GetElementType() );
                            }
                            else
                            {
                                nestedInstance = typeof( IEnumerable ).IsAssignableFrom( memberType )
                                                     ? CreateInstance( memberType )
                                                     : GetInstance( memberType, newDictionary, parentInstance == null ? 0 : parentInstance.GetHashCode() ).Item2;
                            }
                        }

                        if ( isEnumerableType )
                        {
                            var innerType = memberType.GetGenericArguments().FirstOrDefault() ?? memberType.GetElementType();
                            nestedInstance = MaterializeCollection( innerType, newDictionary, nestedInstance, entity );
                        }
                        else
                        {
                            if ( newDictionary.Values.All( v => v == null ) )
                            {
                                nestedInstance = null;
                            }
                            else
                            {
                                nestedInstance = Materialize( newDictionary, nestedInstance, entity );
                            }
                        }

                        SetMemberValue( memberInfo, entity, nestedInstance );
                    }
                }
            }

            return entity;
        }

        /// <summary>
        /// Populates the given instance's properties where the IDictionary key property names
        /// match the type's property names case insensitively.
        /// 
        /// Population of complex nested child properties is supported by underscoring "_" into the
        /// nested child properties in the property name.
        /// </summary>
        /// <param name="type">Underlying instance type</param>
        /// <param name="dictionary">Dictionary of property names and values</param>
        /// <param name="instance">Instance to populate</param>
        /// <param name="parentInstance">Optional parent instance of the instance being populated</param>
        /// <returns>Populated instance</returns>
        internal object MaterializeCollection( Type type, IDictionary<string, object> dictionary, object instance, object parentInstance = null )
        {
            Type baseListType = typeof( List<> );
            Type collectionType = instance == null ? baseListType.MakeGenericType( type ) : instance.GetType();

            if ( instance == null )
            {
                instance = CreateInstance( collectionType );
            }

            // If the dictionnary only contains null values, we return an empty instance
            if ( dictionary.Values.FirstOrDefault( v => v != null ) == null )
            {
                return instance;
            }

            var getInstanceResult = GetInstance( type, dictionary, parentInstance );

            // Is this a newly created instance? If false, then this item was retrieved from the instance cache.
            bool isNewlyCreatedInstance = getInstanceResult.Item1;

            bool isArray = instance.GetType().IsArray;

            object instanceToAddToCollectionInstance = getInstanceResult.Item2;

            instanceToAddToCollectionInstance = Materialize( dictionary, instanceToAddToCollectionInstance, parentInstance );

            if ( isNewlyCreatedInstance )
            {
                if ( isArray )
                {
                    var arrayList = new ArrayList { instanceToAddToCollectionInstance };

                    instance = arrayList.ToArray( type );
                }
                else
                {
                    MethodInfo addMethod = collectionType.GetMethod( "Add" );

                    addMethod.Invoke( instance, new[] { instanceToAddToCollectionInstance } );
                }
            }
            else
            {
                MethodInfo containsMethod = collectionType.GetMethod( "Contains" );

                var alreadyContainsInstance = (bool)containsMethod.Invoke( instance, new[] { instanceToAddToCollectionInstance } );

                if ( alreadyContainsInstance == false )
                {
                    if ( isArray )
                    {
                        var arrayList = new ArrayList( (ICollection)instance );

                        instance = arrayList.ToArray( type );
                    }
                    else
                    {
                        MethodInfo addMethod = collectionType.GetMethod( "Add" );

                        addMethod.Invoke( instance, new[] { instanceToAddToCollectionInstance } );
                    }
                }
            }

            return instance;
        }

        /// <summary>
        /// Creates an instance of the specified type using that type's default constructor.
        /// </summary>
        /// <param name="type">The type of object to create.</param>
        /// <returns>
        /// A reference to the newly created object.
        /// </returns>
        private object CreateInstance( Type type )
        {
            if ( type == typeof( string ) )
            {
                return string.Empty;
            }

            var instance = Activator.CreateInstance( type );

            

            return instance;
        }

        #endregion

        #region Mapping Methods to Remove

        /// <summary>
        /// Gets the Type of the Field or Property
        /// </summary>
        /// <param name="member">FieldInfo or PropertyInfo object</param>
        /// <returns>Type</returns>
        public static Type GetMemberType( object member )
        {
            Type type = null;

            var fieldInfo = member as FieldInfo;

            if ( fieldInfo != null )
            {
                type = fieldInfo.FieldType;
            }
            else
            {
                var propertyInfo = member as PropertyInfo;

                if ( propertyInfo != null )
                {
                    type = propertyInfo.PropertyType;
                }
            }

            return type;
        }

        /// <summary>
        /// Sets the value on a Field or Property
        /// </summary>
        /// <param name="member">FieldInfo or PropertyInfo object</param>
        /// <param name="obj">Object to set the value on</param>
        /// <param name="value">Value</param>
        private void SetMemberValue( object member, object obj, object value )
        {
            var fieldInfo = member as FieldInfo;

            if ( fieldInfo != null )
            {
                value = ConvertValuesTypeToMembersType( value, fieldInfo.Name, fieldInfo.FieldType, fieldInfo.DeclaringType );

                try
                {
                    fieldInfo.SetValue( obj, value );
                }
                catch ( Exception e )
                {
                    string errorMessage =
                        string.Format( "{0}: An error occurred while mapping the value '{1}' of type {2} to the member name '{3}' of type {4} on the {5} class.",
                                       e.Message, value, value.GetType(), fieldInfo.Name, fieldInfo.FieldType, fieldInfo.DeclaringType );

                    throw new Exception( errorMessage, e );
                }
            }
            else
            {
                var propertyInfo = member as PropertyInfo;

                if ( propertyInfo != null )
                {
                    value = ConvertValuesTypeToMembersType( value, propertyInfo.Name, propertyInfo.PropertyType, propertyInfo.DeclaringType );

                    try
                    {
                        propertyInfo.SetValue( obj, value, null );
                    }
                    catch ( Exception e )
                    {
                        string errorMessage =
                            string.Format( "{0}: An error occurred while mapping the value '{1}' of type {2} to the member name '{3}' of type {4} on the {5} class.",
                                           e.Message, value, value.GetType(), propertyInfo.Name, propertyInfo.PropertyType, propertyInfo.DeclaringType );

                        throw new Exception( errorMessage, e );
                    }
                }
            }
        }

        /// <summary>
        /// Converts the values type to the members type if needed.
        /// </summary>
        /// <param name="value">Object value.</param>
        /// <param name="memberName">Member name.</param>
        /// <param name="memberType">Member type.</param>
        /// <param name="classType">Declarying class type.</param>
        /// <returns>Value converted to the same type as the member type.</returns>
        private static object ConvertValuesTypeToMembersType( object value, string memberName, Type memberType, Type classType )
        {
            if ( value == null || value == DBNull.Value )
                return null;

            var valueType = value.GetType();

            try
            {
                if ( valueType != memberType )
                {
                    // TJT: Remove after testing

                    foreach ( var typeConverter in TypeConverters.OrderBy( x => x.Order ) )
                    {
                        if ( typeConverter.CanConvert( value, memberType ) )
                        {
                            var convertedValue = typeConverter.Convert( value, memberType );

                            return convertedValue;
                        }
                    }
                }
            }
            catch ( Exception e )
            {
                string errorMessage = string.Format( "{0}: An error occurred while mapping the value '{1}' of type {2} to the member name '{3}' of type {4} on the {5} class.",
                                                     e.Message, value, valueType, memberName, memberType, classType );

                throw new Exception( errorMessage, e );
            }

            return value;
        }

        /// <summary>
        /// Creates a Dictionary of field or property names and their corresponding FieldInfo or PropertyInfo objects
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Dictionary of member names and member info objects</returns>
        private Dictionary<string, MemberInfo> CreateFieldAndPropertyInfoDictionary( Type type )
        {
            var dictionary = new Dictionary<string, MemberInfo>();

            var properties = type.GetProperties();

            foreach ( var propertyInfo in properties )
            {
                dictionary.Add( propertyInfo.Name, propertyInfo );
            }

            var fields = type.GetFields();

            foreach ( var fieldInfo in fields )
            {
                dictionary.Add( fieldInfo.Name, fieldInfo );
            }

            return dictionary;
        }

        /// <summary>
        /// Gets the value of the member
        /// </summary>
        /// <param name="member">FieldInfo or PropertyInfo object</param>
        /// <param name="obj">Object to get the value from</param>
        /// <returns>Value of the member</returns>
        private object GetMemberValue( object member, object obj )
        {
            object value = null;

            var fieldInfo = member as FieldInfo;

            if ( fieldInfo != null )
            {
                value = fieldInfo.GetValue( obj );
            }
            else
            {
                var propertyInfo = member as PropertyInfo;

                if ( propertyInfo != null )
                {
                    value = propertyInfo.GetValue( obj, null );
                }
            }

            return value;
        }

        /// <summary>
        /// Creates a TypeMap for a given Type.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>TypeMap</returns>
        private EntityTypeMap CreateTypeMap( Type type )
        {
            //var conventionIdentifiers = Configuration.IdentifierConventions.Select( applyIdentifierConvention => applyIdentifierConvention( type ) ).ToList();

            var fieldsAndProperties = CreateFieldAndPropertyInfoDictionary( type );

            var identifiers = new List<string>();

            foreach ( var fieldOrProperty in fieldsAndProperties )
            {
                var memberName = fieldOrProperty.Key;

                var member = fieldOrProperty.Value;

                var fieldInfo = member as FieldInfo;

                if ( fieldInfo != null )
                {
                    //if ( fieldInfo.GetCustomAttributes( Configuration.IdentifierAttributeType, false ).Length > 0 )
                    //{
                    //    identifiers.Add( memberName );
                    //}
                    //else if ( conventionIdentifiers.Exists( x => x.ToLower() == memberName.ToLower() ) )
                    //{
                    //    identifiers.Add( memberName );
                    //}
                }
                else
                {
                    var propertyInfo = member as PropertyInfo;

                    if ( propertyInfo != null )
                    {
                        //if ( propertyInfo.GetCustomAttributes( Configuration.IdentifierAttributeType, false ).Length > 0 )
                        //{
                        //    identifiers.Add( memberName );
                        //}
                        //else if ( conventionIdentifiers.Exists( x => x.ToLower() == memberName.ToLower() ) )
                        //{
                        //    identifiers.Add( memberName );
                        //}
                    }
                }
            }

            var typeMap = new EntityTypeMap( type, identifiers, fieldsAndProperties );

            return typeMap;
        }
        /// <summary>
        /// Gets the identifiers for the given type. Returns NULL if not found.
        /// Results are cached for subsequent use and performance.
        /// </summary>
        /// <remarks>
        /// If no identifiers have been manually added, this method will attempt
        /// to first find an <see cref="Slapper.AutoMapper.Id"/> attribute on the <paramref name="type"/>
        /// and if not found will then try to match based upon any specified identifier conventions.
        /// </remarks>
        /// <param name="type">Type</param>
        /// <returns>Identifier</returns>
        private IEnumerable<string> GetIdentifiers( Type type )
        {
            var typeMap = TypeMapCache.GetOrAdd( type, CreateTypeMap( type ) );

            return typeMap.Identifiers.Any() ? typeMap.Identifiers : null;
        }

        /// <summary>
        /// Get a Dictionary of a type's property names and field names and their corresponding PropertyInfo or FieldInfo.
        /// Results are cached for subsequent use and performance.
        /// </summary>
        /// <param name="type">Type</param>
        /// <returns>Dictionary of a type's property names and their corresponding PropertyInfo</returns>
        public Dictionary<string, MemberInfo> GetFieldsAndProperties( Type type )
        {
            var typeMap = TypeMapCache.GetOrAdd( type, CreateTypeMap( type ) );

            return typeMap.PropertiesAndFieldsInfo;
        }
        #endregion

        #region Private Caching Methods

        private void ClearInstanceCache()
        {
            // TJT: FIXME - no instance thread cache.
            //InternalHelpers.ContextStorage.Remove( InstanceCacheContextStorageKey );
        }

        /// <summary>
        /// Computes a key for storing and identifying an instance in the cache.
        /// </summary>
        /// <param name="type">Type of instance to get</param>
        /// <param name="properties">List of properties and values</param>
        /// <param name="parentInstance">Parent instance. Can be NULL if this is the root instance.</param>
        /// <returns>
        /// InstanceKey that will be unique for given set of identifiers values for the type. If the type isn't associated with any 
        /// identifier, the return value is made unique by generating a Guid.
        /// ASSUMES GetIdentifiers(type) ALWAYS RETURN IDENTIFIERS IN THE SAME ORDER FOR A GIVEN TYPE.
        /// This is certainly the case as long as GetIdentifiers caches its result for a given type (which it does by 2016-11-25).
        /// </returns>
        private InstanceKey GetCacheKey( Type type, IDictionary<string, object> properties, object parentInstance )
        {
            var identifierValues = GetIdentifiers( type )?.Select( id => properties[ id ] ).DefaultIfEmpty( Guid.NewGuid() ).ToArray()
                ?? new object[] { Guid.NewGuid() };

            var key = new InstanceKey( type, identifierValues, parentInstance );

            return key;
        }

        /// <summary>
        /// Gets a new or existing instance depending on whether an instance with the same identifiers already existing
        /// in the instance cache.
        /// </summary>
        /// <param name="type">Type of instance to get</param>
        /// <param name="properties">List of properties and values</param>
        /// <param name="parentInstance">Parent instance. Can be NULL if this is the root instance.</param>
        /// <returns>
        /// Tuple of bool, object, int where bool represents whether this is a newly created instance,
        /// object being an instance of the requested type and int being the instance's identifier hash.
        /// </returns>
        private Tuple<bool, object, InstanceKey> GetInstance( Type type, IDictionary<string, object> properties, object parentInstance = null )
        {
            var key = GetCacheKey( type, properties, parentInstance );

            var instanceCache = GetInstanceCache();

            object instance;

            var isNewlyCreatedInstance = !instanceCache.TryGetValue( key, out instance );

            if ( isNewlyCreatedInstance )
            {
                instance = CreateInstance( type );
                instanceCache[ key ] = instance;
            }

            return Tuple.Create( isNewlyCreatedInstance, instance, key );
        }

        public struct InstanceKey : IEquatable<InstanceKey>
        {
            /// <summary>
            /// Combine several hashcodes into a single new one. This implementation was grabbed from http://stackoverflow.com/a/34229665 where it is introduced 
            /// as MS implementation of GetHashCode() for strings.
            /// </summary>
            /// <param name="hashCodes">Hascodes to be combined.</param>
            /// <returns>A new Hascode value combining those passed as parameters.</returns>
            internal int CombineHashCodes( params int[] hashCodes )
            {
                int hash1 = (5381 << 16) + 5381;
                int hash2 = hash1;

                int i = 0;
                foreach ( var hashCode in hashCodes )
                {
                    if ( i % 2 == 0 )
                        hash1 = ((hash1 << 5) + hash1 + (hash1 >> 27)) ^ hashCode;
                    else
                        hash2 = ((hash2 << 5) + hash2 + (hash2 >> 27)) ^ hashCode;

                    ++i;
                }

                return hash1 + (hash2 * 1566083941);
            }
            public bool Equals( InstanceKey other )
            {
                return Equals( Type, other.Type )
                    && Equals( ParentInstance, other.ParentInstance )
                    && StructuralComparisons.StructuralEqualityComparer.Equals( IdentifierValues, other.IdentifierValues );
            }

            public override bool Equals( object obj )
            {
                if ( ReferenceEquals( null, obj ) ) return false;
                return obj is InstanceKey && Equals( (InstanceKey)obj );
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return CombineHashCodes( Type?.GetHashCode() ?? 0, StructuralComparisons.StructuralEqualityComparer.GetHashCode( IdentifierValues ), ParentInstance?.GetHashCode() ?? 0 );
                }
            }

            public static bool operator ==( InstanceKey left, InstanceKey right ) { return left.Equals( right ); }

            public static bool operator !=( InstanceKey left, InstanceKey right ) { return !left.Equals( right ); }

            public InstanceKey( Type type, object[] identifierValues, object parentInstance )
            {
                Type = type;
                IdentifierValues = identifierValues;
                ParentInstance = parentInstance;
            }

            public Type Type { get; }
            public object[] IdentifierValues { get; }
            public object ParentInstance { get; }
        }

        private Dictionary<InstanceKey, object> GetInstanceCache()
        {
            if ( _instanceCache == null )
            {
                _instanceCache = new Dictionary<InstanceKey, object>();

                // TJT Remove after testing
                //InternalHelpers.ContextStorage.Store( InstanceCacheContextStorageKey, instanceCache );
            }

            return _instanceCache;
        }

        #endregion
    }
}
