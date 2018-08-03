#region Namespaces

using Achilles.Entities.Sqlite.Configuration;
using Achilles.Entities.Storage;
using Microsoft.Data.Sqlite;
using System.Diagnostics;
using System.IO;

#endregion

namespace Achilles.Entities.Sqlite.Storage
{
    public class SqliteRelationalDatabase : RelationalDatabase
    {
        #region Private Fields

        private const int SQLITE_CANTOPEN = 14;
        private bool _wasConnectionOpened;

        #endregion

        #region Constructor(s)

        /// <summary>
        /// Constructs a new Sqlite relational database service.
        /// </summary>
        /// <param name="dbContext">Injected DbContext service which contains a reference to the actual DbContext.</param>
        /// <param name="connection">Injected Sqlite connection service.</param>
        /// <param name="creator">Injected Sqlite database creator service.</param>
        public SqliteRelationalDatabase( 
            IDbContextService dbContext, 
            IRelationalConnection connection,
            IRelationalDatabaseCreator creator )
            : base( dbContext, connection, creator )
        {
            // The Sqlite database service maintains an open database connection for the lifetime of the DbContext.
            // We use the connection state status to determine if the connection should be closed when the database is disposed.

            Trace.WriteLine( "SqliteRelationalDatabase constructor: Opening connection." );

            _wasConnectionOpened = Connection.Open();

            Trace.WriteLine( "Connection opened internally is " + _wasConnectionOpened.ToString() );
        }

        #endregion

        #region Public Methods

        public override bool Exists()
        {
            var options = _dbContext.Instance.Options as SqliteOptions;

            var readOnlyConnectionStringBuilder = new SqliteConnectionStringBuilder( options.ConnectionString )
            {
                Mode = SqliteOpenMode.ReadOnly
            };

            using ( var readOnlyConnection = new SqliteConnection( readOnlyConnectionStringBuilder.ToString() ) )
            {
                try
                {
                    readOnlyConnection.Open();
                }
                catch ( SqliteException ex ) when ( ex.SqliteErrorCode == SQLITE_CANTOPEN )
                {
                    return false;
                }
            }

            return true;
        }
        
        /// <summary>
        /// Creates the database file.
        /// </summary>
        public override void Create()
        {
            Trace.WriteLine( "Entering SqliteRelationalDatabase::Create()" );
            Connection.Open();
            Connection.Close();
        }

        public override bool Delete()
        {
            if ( Exists() )
            {
                string path = null;

                Connection.Open();

                try
                {
                    path = Connection.DbConnection.DataSource;
                }
                catch
                {
                    // any exceptions here can be ignored
                }
                finally
                {
                    Connection.Close();
                }

                if ( !string.IsNullOrEmpty( path ) )
                {
                    File.Delete( path );
                }

                return true;
            }

            return false;
        }

        public override bool HasTables()
        {
            var sqliteCommand = Connection.DbConnection.CreateCommand();
            sqliteCommand.CommandText = "SELECT COUNT(*) FROM \"sqlite_master\" WHERE \"type\" = 'table' AND \"rootpage\" IS NOT NULL;";

            Debug.Assert( Connection.DbConnection.State == System.Data.ConnectionState.Open );

            var count = (long)sqliteCommand.ExecuteScalar();

            return count != 0;
        }

        #endregion
    }
}
