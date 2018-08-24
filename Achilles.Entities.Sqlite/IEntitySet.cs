#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using System;
using System.Collections.Generic;

#endregion

namespace Achilles.Entities
{
    /// <summary>
    /// Internal API for <see cref="EntitySet{TEntity}"/>
    /// </summary>
    /// <remarks>An internal interface for now.</remarks>
    public interface IEntitySet
    {
        /// <summary>
        /// Returns the entity set generic type.
        /// </summary>
        /// <returns></returns>
        Type EntityType { get; }

        //IEnumerable<TSource> GetSource<TSource>( TSource source ) where TSource : class;
    }
}
