#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using System.Collections.Generic;

#endregion

namespace Achilles.Entities.Extensions
{
    public static class DictionaryExtensions
    {
        internal static void AddOrUpdate<TKey, TValue>( this IDictionary<TKey, IList<TValue>> dict, TKey key, TValue value )
        {
            if ( dict.TryGetValue( key, out var list ) )
            {
                list.Add( value );
            }
            else
            {
                dict.Add( key, new List<TValue> { value } );
            }
        }
    }
}
