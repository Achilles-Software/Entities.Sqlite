#region Copyright Notice

// Copyright (c) by Achilles Software, All rights reserved.
//
// Licensed under the MIT License. See License.txt in the project root for license information.
//
// Send questions regarding this copyright notice to: mailto:todd.thomson@achilles-software.com

#endregion

#region Namespaces

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Achilles.Entities.Properties;
using Achilles.Entities.Relational;
using Achilles.Entities.Relational.Configuration;

#endregion

namespace Achilles.Entities.Storage
{
    public abstract class RelationalConnection : IRelationalConnection
    {
        #region Fields

        protected readonly string _connectionString;
        protected readonly int? _commandTimeout;
        protected readonly DbConnection _connection;

        //TJT: Not sure where this lives yet!
        //private readonly object _concurrencyLock = new object(); 

        protected bool _isConnectionOwned;

        protected int _openCount = 0;

        #endregion

        #region Constructor(s)

        protected RelationalConnection( IDataContextService dbContext )
        {
            var options = dbContext.Instance.Options as IRelationalOptions;

            if ( options.Connection != null )
            {
                _connection = options.Connection;
                _isConnectionOwned = false;
            }
            else if ( !string.IsNullOrWhiteSpace( options.ConnectionString ) )
            {
                _connectionString = options.ConnectionString;
                _connection = CreateDbConnection();

                _isConnectionOwned = true;
            }
            else
            {
                throw new InvalidOperationException( Resources.NoConnectionOrConnectionString );
            }

            _commandTimeout = options.CommandTimeout;
        }

        #endregion

        #region Public/Protected Properties

        public virtual Guid ConnectionId { get; } = Guid.NewGuid();

        protected abstract DbConnection CreateDbConnection();

        public virtual string ConnectionString => _connectionString ?? DbConnection.ConnectionString;

        public virtual DbConnection DbConnection => _connection;

        public int? CommandTimeout => _commandTimeout;

        #endregion

        #region Public Methods

        /// <summary>
        /// Opens a connection to the database if the <see cref="ConnectionState"/> is not already open.  
        /// </summary>
        /// <returns>True if the <see cref="DbConnection"/> was actually opened. False otherwise. </returns>
        public virtual bool Open()
        {
            Trace.WriteLine( "---> Entering Open()" );

            if ( DbConnection.State == ConnectionState.Broken )
            {
                DbConnection.Close();
            }

            var wasOpened = false;

            if ( DbConnection.State != ConnectionState.Open )
            {
                Trace.WriteLine( "Opening DbConnection" );
                DbConnection.Open();

                wasOpened = true;
            }

            _openCount++;

            Trace.WriteLine( "Open count now: " + _openCount.ToString() );

            return wasOpened;
        }

        public virtual Task<bool> OpenAsync( CancellationToken cancellationToken )
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Closes a connection to the database if the open count on the connection
        /// to the database is zero and if the connection was created internally.
        /// </summary>
        /// <returns>True if the connection was actually closed; false otherwise.</returns>
        public virtual bool Close()
        {
            Trace.WriteLine( "---> Entering Close()" );
            var wasClosed = false;

            if ( (_isConnectionOwned && (_openCount > 0)) && (--_openCount == 0) )
            {
                if ( DbConnection.State != ConnectionState.Closed )
                {
                    Trace.WriteLine( "Closing DbConnection" );
                    DbConnection.Close();

                   wasClosed = true;
                }
            }

            Trace.WriteLine( "Open count now: " + _openCount.ToString() );

            return wasClosed;
        }

        public abstract  object ExecuteScalar( String commandText, IReadOnlyDictionary<String, object> parameterValues );

        public abstract int ExecuteNonQuery( String commandText, IReadOnlyDictionary<String, object> parameterValues );

        public abstract IEnumerable<Dictionary<string,object>> ExecuteReader( string sql, SqlParameterCollection parameters, IDbTransaction transaction );

        public abstract int LastInsertRowId();

        #endregion

        #region IDisposable Support

        private bool disposedValue = false;

        protected virtual void Dispose( bool disposing )
        {
            if ( !disposedValue )
            {
                if ( disposing )
                {
                    Trace.WriteLine( "RelationalConnection disposing." );
                    // TODO: dispose managed state (managed objects).
                    if ( _isConnectionOwned )
                    {
                        Trace.WriteLine( "DbConnection Disposing." );
                        DbConnection?.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose( true );
        }

        #endregion
    }
}
