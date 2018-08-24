#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Modelling.Mapping.Accessors
{
    public abstract class MemberAccessor<TEntity, TValue>
    {
        protected Func<TEntity, object> _getter;
        protected Action<TEntity, object> _setter;

        public MemberAccessor( MemberInfo memberInfo )
        {
            MemberInfo = memberInfo ?? throw new System.ArgumentNullException( nameof( memberInfo ) );

            CreateGetter();
            CreateSetter();
        }

        protected MemberInfo MemberInfo { get; }

        public virtual object GetValue( TEntity entity ) => _getter( entity );

        public virtual void SetValue( TEntity entity, object value ) => _setter( entity, value );

        private void CreateGetter()
        {
            if ( MemberInfo is PropertyInfo propertyInfo )
            {
                ParameterExpression instance = Expression.Parameter( typeof( TEntity ), "instance" );

                var body = Expression.Call( instance, propertyInfo.GetGetMethod() );
                var parameters = new ParameterExpression[] { instance };
                Expression conversion = Expression.Convert( body, typeof( object ) );

                _getter = Expression.Lambda<Func<TEntity, object>>( conversion, parameters ).Compile();
            }
            else if ( MemberInfo is FieldInfo field )
            {
                ParameterExpression instance = Expression.Parameter( typeof( TEntity ), "instance" );

                MemberExpression fieldExpression = Expression.Field( instance, field );
                var parameters = new ParameterExpression[] { instance };
                Expression conversion = Expression.Convert( fieldExpression, typeof( object ) );

                _getter = Expression.Lambda<Func<TEntity, object>>( conversion, parameters ).Compile();
            }
        }

        private void CreateSetter()
        {
            if ( MemberInfo is PropertyInfo propertyInfo )
            {
                var columnPropertySetMethod = propertyInfo.GetSetMethod();
                var setMethodParameterType = columnPropertySetMethod.GetParameters()[0].ParameterType;

                var entityInstanceParameter = Expression.Parameter( typeof( TEntity ), "instance" );
                var valueParameter = Expression.Parameter( typeof( object ), "value" );
                Expression conversion = Expression.Convert( valueParameter, setMethodParameterType );

                var body = Expression.Call( entityInstanceParameter, columnPropertySetMethod, conversion );
                var parameters = new ParameterExpression[] { entityInstanceParameter, valueParameter };

                _setter = Expression.Lambda<Action<TEntity, object>>(
                    body, parameters ).Compile();
            }
            else if ( MemberInfo is FieldInfo field )
            {
                var instanceParameter = Expression.Parameter( typeof( TEntity ), "instance" );
                var valueParameter = Expression.Parameter( typeof( object ), "value" );
                Expression conversion = Expression.Convert( valueParameter, field.FieldType );

                MemberExpression fieldExpression = Expression.Field( instanceParameter, field );
                BinaryExpression assignExp = Expression.Assign( fieldExpression, conversion );

                _setter = Expression.Lambda<Action<TEntity, object>>(
                    assignExp, instanceParameter, valueParameter ).Compile();
            }
        }
    }
}
