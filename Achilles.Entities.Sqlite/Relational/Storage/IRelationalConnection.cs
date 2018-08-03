#region Namespaces

using Achilles.Entities.Relational;
using Achilles.Entities.Relational.Query.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Achilles.Entities.Storage
{
    public interface IRelationalConnection : IDisposable
    {
        string ConnectionString { get; }

        int? CommandTimeout { get; }

        DbConnection DbConnection { get; }

        Guid ConnectionId { get; }

        /// <summary>
        /// Opens the connection to the database.
        /// </summary>
        /// <returns>True if the connection not previously opened.</returns>
        bool Open();

        Task<bool> OpenAsync( CancellationToken cancellationToken );

        /// <summary>
        /// Closes the connection to the database.
        /// </summary>
        /// <returns> True if the underlying connection was actually closed; false otherwise. </returns>
        bool Close();

        int ExecuteNonQuery( string commandText, IReadOnlyDictionary< string, object> parameters );

        object ExecuteScalar( string commandText, IReadOnlyDictionary<string, object> parameters );

        IEnumerable<dynamic> ExecuteReader( string sql, SqlParameterCollection parameters, IDbTransaction transaction );

        int LastInsertRowId();
    }
}