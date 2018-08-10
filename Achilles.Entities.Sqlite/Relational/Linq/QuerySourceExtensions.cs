#region Namespaces

using Remotion.Linq.Clauses;
using System;

#endregion

namespace Achilles.Entities.Relational.Linq
{
    internal static class QuerySourceExtensions
    {
        public static bool HasGeneratedItemName( this IQuerySource querySource )
        {
            if ( querySource == null )
            {
                throw new ArgumentNullException( nameof( querySource ) );
            }

            return querySource.ItemName.StartsWith( "<generated>_", StringComparison.Ordinal );
        }
    }
}
