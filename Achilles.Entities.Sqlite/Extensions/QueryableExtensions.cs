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

using Achilles.Entities.Relational.Query;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace Achilles.Entities.Extensions
{
    //public static class QueryableExtensions
    //{
    //    public static IAsyncEnumerable<TSource> AsAsyncEnumerable<TSource>(
    //        this IQueryable<TSource> source )
    //    {
    //        if ( source is IAsyncEnumerable<TSource> enumerable )
    //        {
    //            return enumerable;
    //        }

    //        if ( source is IAsyncEnumerableAccessor<TSource> entityQueryableAccessor )
    //        {
    //            return entityQueryableAccessor.AsyncEnumerable;
    //        }

    //        throw new InvalidOperationException( "IQueryable not async" );
    //    }
    //}
}
