#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

// Portions of this source were derived from EntityFrameworkCore. See copyright below. Thank-you!

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

#endregion

#region Namespaces

using Achilles.Entities.Extensions;
using Achilles.Entities.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Achilles.Entities.Linq
{
    public static class AsyncQueryableExtensions
    {
        public static Task<TSource> FirstAsync<TSource>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, bool>> predicate,
            CancellationToken cancellationToken = default )
        {
            if ( source == null )
            {
                throw new ArgumentNullException( nameof( source ) );
            }

            if ( predicate == null )
            {
                throw new ArgumentNullException( nameof( predicate ) );
            }

            return ExecuteAsync<TSource, TSource>( _firstPredicate, source, predicate, cancellationToken );
        }

        public static Task<List<TSource>> ToListAsync<TSource>(
            this IQueryable<TSource> source,
            CancellationToken cancellationToken = default )
        {
            if ( source.Provider is IAsyncQueryProvider provider )
            {
                var toListMethodInfo = typeof( Enumerable ).GetMethod( "ToList" )
                    .MakeGenericMethod( new Type[] { source.ElementType } );

                return provider.ExecuteAsync<List<TSource>>(
                        Expression.Call(
                            instance: null,
                            method: toListMethodInfo,
                            arguments: source.Expression ),
                        cancellationToken );

                //return provider.ToListAsync<TSource>( source, cancellationToken );
            }

            throw new InvalidOperationException( "Provider is not async" );
        }

        #region Private

        private static readonly MethodInfo _first = GetMethod( nameof( Queryable.First ) );
        private static readonly MethodInfo _firstPredicate = GetMethod( nameof( Queryable.First ), parameterCount: 1 );

        private static Task<TResult> ExecuteAsync<TSource, TResult>(
            MethodInfo operatorMethodInfo,
            IQueryable<TSource> source,
            CancellationToken cancellationToken = default )
        {
            if ( source.Provider is IAsyncQueryProvider provider )
            {
                if ( operatorMethodInfo.IsGenericMethod )
                {
                    operatorMethodInfo = operatorMethodInfo.MakeGenericMethod( typeof( TSource ) );
                }

                return provider.ExecuteAsync<TResult>(
                    Expression.Call(
                        instance: null,
                        method: operatorMethodInfo,
                        arguments: source.Expression ),
                    cancellationToken );
            }

            throw new InvalidOperationException( "Provider is not async" );
        }

        private static Task<TResult> ExecuteAsync<TSource, TResult>(
            MethodInfo operatorMethodInfo,
            IQueryable<TSource> source,
            LambdaExpression expression,
            CancellationToken cancellationToken = default )
            => ExecuteAsync<TSource, TResult>(
                operatorMethodInfo, source, Expression.Quote( expression ), cancellationToken );

        private static Task<TResult> ExecuteAsync<TSource, TResult>(
            MethodInfo operatorMethodInfo,
            IQueryable<TSource> source,
            Expression expression,
            CancellationToken cancellationToken = default )
        {
            if ( source.Provider is IAsyncQueryProvider provider )
            {
                operatorMethodInfo
                    = operatorMethodInfo.GetGenericArguments().Length == 2
                        ? operatorMethodInfo.MakeGenericMethod( typeof( TSource ), typeof( TResult ) )
                        : operatorMethodInfo.MakeGenericMethod( typeof( TSource ) );

                return provider.ExecuteAsync<TResult>(
                    Expression.Call(
                        instance: null,
                        method: operatorMethodInfo,
                        arguments: new[] { source.Expression, expression } ),
                    cancellationToken );
            }

            throw new InvalidOperationException( "Provider is not async" );
        }

        private static MethodInfo GetMethod<TResult>(
            string name, int parameterCount = 0, Func<MethodInfo, bool> predicate = null )
            => GetMethod(
                name,
                parameterCount,
                mi => mi.ReturnType == typeof( TResult )
                      && (predicate == null || predicate( mi )) );

        private static MethodInfo GetMethod(
            string name, int parameterCount = 0, Func<MethodInfo, bool> predicate = null )
            => typeof( Queryable ).GetTypeInfo().GetDeclaredMethods( name )
                .Single(
                    mi => mi.GetParameters().Length == parameterCount + 1
                          && (predicate == null || predicate( mi )) );

        #endregion
    }
}
