using System;
using System.Collections.Generic;
using System.Text;

namespace Achilles.Entities.Extensions
{
    public static class ListExtensions
    {
        public static void AddIfNotNull<T>( this ICollection<T> list, T element )
        {
            if ( list == null || element == null )
            {
                return;
            }

            list.Add( element );
        }
    }
}
