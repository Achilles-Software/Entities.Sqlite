#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

#endregion

namespace Achilles.Entities.Linq
{
    public static class EagerFetchingExtensions
    {
        public static FluentFetchRequest<TOriginating, TRelated> FetchMany<TOriginating, TRelated>(
            this IQueryable<TOriginating> query, 
            Expression<Func<TOriginating, IEnumerable<TRelated>>> relatedObjectSelector )
        {
            if ( query == null )
            {
                throw new ArgumentNullException( nameof( query ) );
            }

            if ( relatedObjectSelector == null )
            {
                throw new ArgumentNullException( nameof( relatedObjectSelector ) );
            }

            var methodInfo = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod( typeof( TOriginating ), typeof( TRelated ) );

            return CreateFluentFetchRequest<TOriginating, TRelated>( methodInfo, query, relatedObjectSelector );
        }

        public static FluentFetchRequest<TOriginating, TRelated> FetchOne<TOriginating, TRelated>(
            this IQueryable<TOriginating> query, 
            Expression<Func<TOriginating, TRelated>> relatedObjectSelector )
        {
            if ( query == null )
            {
                throw new ArgumentNullException( nameof( query ) );
            }
            if ( relatedObjectSelector == null )
            {
                throw new ArgumentNullException( nameof( relatedObjectSelector ) );
            }

            var methodInfo = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod( typeof( TOriginating ), typeof( TRelated ) );

            return CreateFluentFetchRequest<TOriginating, TRelated>( methodInfo, query, relatedObjectSelector );
        }

        public static FluentFetchRequest<TQueried, TRelated> ThenFetchMany<TQueried, TFetch, TRelated>(
            this FluentFetchRequest<TQueried, TFetch> query,
            Expression<Func<TFetch, IEnumerable<TRelated>>> relatedObjectSelector )
        {
            if ( query == null )
            {
                throw new ArgumentNullException( nameof( query ) );
            }
            if ( relatedObjectSelector == null )
            {
                throw new ArgumentNullException( nameof( relatedObjectSelector ) );
            }

            var methodInfo = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod( typeof( TQueried ), typeof( TFetch ), typeof( TRelated ) );

            return CreateFluentFetchRequest<TQueried, TRelated>( methodInfo, query, relatedObjectSelector );
        }

        public static FluentFetchRequest<TQueried, TRelated> ThenFetchOne<TQueried, TFetch, TRelated>(
            this FluentFetchRequest<TQueried, TFetch> query,
            Expression<Func<TFetch, TRelated>> relatedObjectSelector )
        {
            if ( query == null )
            {
                throw new ArgumentNullException( nameof( query ) );
            }
            if ( relatedObjectSelector == null )
            {
                throw new ArgumentNullException( nameof( relatedObjectSelector ) );
            }

            var methodInfo = ((MethodInfo)MethodBase.GetCurrentMethod()).MakeGenericMethod( typeof( TQueried ), typeof( TFetch ), typeof( TRelated ) );

            return CreateFluentFetchRequest<TQueried, TRelated>( methodInfo, query, relatedObjectSelector );
        }

        private static FluentFetchRequest<TOriginating, TRelated> CreateFluentFetchRequest<TOriginating, TRelated>(
            MethodInfo currentFetchMethod,
            IQueryable<TOriginating> query,
            LambdaExpression relatedObjectSelector )
        {
            var queryProvider = query.Provider; // ArgumentUtility.CheckNotNullAndType<QueryProviderBase>( "query.Provider", query.Provider );

            var callExpression = Expression.Call( currentFetchMethod, query.Expression, relatedObjectSelector );

            return new FluentFetchRequest<TOriginating, TRelated>( queryProvider, callExpression );
        }
    }
}
