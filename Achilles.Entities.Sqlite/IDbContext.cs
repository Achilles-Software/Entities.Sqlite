#region Namespaces

using System;
using System.Collections.Generic;
using System.Data;

#endregion

namespace Achilles.Entities
{
    public interface IDbContext : IDisposable
    {
        IDbConnection Connection { get; }

        //IEnumerable<T> Query<T>( string sql ) where T : class, new();
        //IEnumerable<T> Query<T>( string sql, object parameters ) where T : class, new();
    }
}
