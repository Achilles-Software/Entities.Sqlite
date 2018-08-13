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
