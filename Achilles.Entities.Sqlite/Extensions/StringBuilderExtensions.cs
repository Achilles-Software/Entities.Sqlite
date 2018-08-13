#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:Todd.Thomson@achilles-software.com

#endregion

#region Namespaces

using System.Collections.Generic;
using System.Text;

#endregion

namespace Achilles.Entities.Extensions
{
    public static class StringBuilderExtensions
    {
        public static void AppendEnumerable( this StringBuilder stringBuilder, IEnumerable<string> e )
        {
            foreach ( var item in e )
            {
                stringBuilder.Append( item );
            }
        }

        public static void AppendEnumerable( this StringBuilder stringBuilder, IEnumerable<string> e, string delimiter )
        {
            bool first = true;

            foreach ( var item in e )
            {
                if ( first )
                {
                    first = false;
                    stringBuilder.Append( item );
                }
                else
                {
                    stringBuilder.AppendFormat( "{0}{1}", delimiter, item );
                }
            }
        }

        public static void AppendEnumerable( this StringBuilder stringBuilder, IEnumerable<string> e, string prefix, string delimiter )
        {
            bool first = true;

            foreach ( var item in e )
            {
                if ( first )
                {
                    first = false;
                    stringBuilder.AppendFormat( "{0}{1}", prefix, item );
                }
                else
                {
                    stringBuilder.AppendFormat( "{0}{1}", delimiter, item );
                }
            }
        }
    }
}
