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
            }

            throw new InvalidOperationException( "Provider is not async" );
        }

        /// <summary>
        /// Async version of Queryable.Any with predicate arg.
        /// </summary>
        public static Task<bool> AnyAsync<TSource>(
            this IQueryable<TSource> source, 
            Expression<Func<TSource, bool>> predicate,
            CancellationToken cancellationToken = default )
        {
            if ( source == null )
            {
                throw new ArgumentNullException( "source" );
            }
            if ( predicate == null )
            {
                throw new ArgumentNullException( "predicate" );
            }

            if ( source.Provider is IAsyncQueryProvider provider )
            {
                return provider.ExecuteAsync<bool>(
                    Expression.Call( 
                        GetMethod( nameof( Queryable.Any ), parameterCount: 1 ).MakeGenericMethod( typeof( TSource ) ),
                        source.Expression, 
                        predicate ),
                    cancellationToken );
            }

            throw new InvalidOperationException( "Query provider does not support async." );
        }

        /// <summary>
        /// Async version of Queryable.Count.
        /// </summary>
        public static Task<int> CountAsync<TSource>(
            this IQueryable<TSource> source,
            CancellationToken cancellationToken = default )
        {
            if ( source == null )
            {
                throw new ArgumentNullException( "source" );
            }

            if ( source.Provider is IAsyncQueryProvider provider )
            {
                return provider.ExecuteAsync<int>(
                    Expression.Call(
                        GetMethod( nameof( Queryable.Count ) ).MakeGenericMethod( typeof( TSource ) ),
                        source.Expression ),
                    cancellationToken );
            }

            throw new InvalidOperationException( "Query provider does not support async." );
        }

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

            if ( source.Provider is IAsyncQueryProvider provider )
            {
                return provider.ExecuteAsync<TSource>(
                    Expression.Call(
                        GetMethod( nameof( Queryable.First ), parameterCount: 1 ).MakeGenericMethod( typeof( TSource ) ),
                        source.Expression,
                        predicate ),
                    cancellationToken );
            }

            throw new InvalidOperationException( "Query provider does not support async." );
        }

        /// <summary>
        /// Async version of Queryable.Sum.
        /// </summary>
        public static Task<decimal> SumAsync<TSource>(
            this IQueryable<TSource> source,
            Expression<Func<TSource, decimal>> predicate,
            CancellationToken cancellationToken = default )
        {
            if ( source == null )
            {
                throw new ArgumentNullException( "source" );
            }
            if ( predicate == null )
            {
                throw new ArgumentNullException( nameof( predicate ) );
            }

            if ( source.Provider is IAsyncQueryProvider provider )
            {
                return provider.ExecuteAsync<decimal>(
                    Expression.Call(
                        GetMethod( nameof( Queryable.Sum ), parameterCount: 1 ).MakeGenericMethod( typeof( TSource ) ),
                        source.Expression,
                        predicate ),
                    cancellationToken );
            }

            throw new InvalidOperationException( "Query provider does not support async." );
        }

        #region Private

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
