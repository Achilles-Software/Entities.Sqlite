using System;
using System.Collections.Generic;
using System.Text;

namespace Achilles.Entities.Helpers
{
    internal static class DictionaryExtensions
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
