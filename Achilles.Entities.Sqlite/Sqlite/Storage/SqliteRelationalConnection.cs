#region Namespaces

using Achilles.Entities.Relational.Modelling.Mapping;
using Achilles.Entities.Relational;
using Achilles.Entities.Sqlite.Statements.Insert;
using Achilles.Entities.Storage;
using Microsoft.Data.Sqlite;
using SQLitePCL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

#endregion

namespace Achilles.Entities.Sqlite.Storage
{
    public class SqliteRelationalConnection : RelationalConnection
    {
        #region Fields

        private readonly bool _enforceForeignKeys = true;
        private readonly object _concurrencyLock = new object(); 

        #endregion

        #region Constructor(s)

        public SqliteRelationalConnection( IDataContextService dbContext )
            : base( dbContext )
        {
        }

        #endregion

        protected override DbConnection CreateDbConnection() => new SqliteConnection( ConnectionString );

        //public virtual IRelationalConnection CreateReadOnlyConnection()
        //{
        //    var connectionStringBuilder = new SqliteConnectionStringBuilder( ConnectionString )
        //    {
        //        Mode = SqliteOpenMode.ReadOnly
        //    };

        //    return new SqliteRelationalConnection( connectionStringBuilder.ConnectionString );
        //}

        public override int LastInsertRowId()
        {
            var connection = DbConnection as SqliteConnection;

            return (int)raw.sqlite3_last_insert_rowid( connection.Handle );
        }

        public override bool Open()
        {
            if ( base.Open() )
            {
                EnableForeignKeys();

                return true;
            }

            return false;
        }

        public override async Task<bool> OpenAsync( CancellationToken cancellationToken )
        {
            if ( await base.OpenAsync( cancellationToken ) )
            {
                EnableForeignKeys();

                return true;
            }

            return false;
        }

        public override object ExecuteScalar(
            string commandText,
            IReadOnlyDictionary<string, object> parameterValues )
        {
            var scalarCommand = CreateCommand( commandText, parameterValues );
            
            return scalarCommand.ExecuteScalar();
        }

        public override int ExecuteNonQuery( 
            string commandText, IReadOnlyDictionary<string, object> parameterValues )
        {
            var nonQueryCommand = CreateCommand( commandText, parameterValues );
            
            return nonQueryCommand.ExecuteNonQuery();
        }

        public override IEnumerable<dynamic> ExecuteReader( string sql, SqlParameterCollection parameters, IDbTransaction transaction )
        {
            var queryCommand = CreateCommand( sql, parameters.ToDictionary() );
            
            DbDataReader reader = null;

            reader = queryCommand.ExecuteReader();

            if ( reader == null )
            {
                yield return new ExpandoObject() as IDictionary<string, object>;
            }
            else
            {
                var names = Enumerable.Range( 0, reader.FieldCount ).Select( reader.GetName ).ToList();

                foreach ( IDataRecord record in reader )
                {
                    var expando = new ExpandoObject() as IDictionary<string, object>;

                    foreach ( var name in names )
                        expando[ name ] = record[ name ];

                    yield return expando;
                }
            }
        }

        public virtual int DefaultTimeout { get; set; } = 30;

        private void EnableForeignKeys()
        {
            var command = DbConnection.CreateCommand();

            string commandText;

            if ( _enforceForeignKeys )
            {
                commandText = "PRAGMA foreign_keys=ON;";
            }
            else
            {
                commandText = "PRAGMA foreign_keys=OFF;";
            }

            command.CommandText = commandText;

            command.ExecuteNonQuery();
        }

        protected virtual DbCommand CreateCommand(
            string commandText,
            IReadOnlyDictionary<string, object> parameterValues )
        {
            var command = DbConnection.CreateCommand();

            command.CommandText = commandText;

            // TODO: Add connection transactions

            //if ( CurrentTransaction != null )
            //{
            //    command.Transaction = connection.CurrentTransaction.GetDbTransaction();
            //}

            if ( CommandTimeout != null )
            {
                command.CommandTimeout = (int)CommandTimeout;
            }

            foreach ( var parameterName in parameterValues.Keys )
            {
                var parameter = command.CreateParameter();
                parameter.ParameterName = parameterName;
                parameter.Value = parameterValues[ parameterName ];

                command.Parameters.Add( parameter );
            }

            return command;
        }

        #region Internal Threading/Concurrency Methods

        internal Task<TResult> ExecuteAsync<TResult>( Func<TResult> dbMethod, CancellationToken cancellationToken )
        {
            return Task.Factory.StartNew( () =>
            {
                lock ( _concurrencyLock )
                {
                    return dbMethod();
                }
            }, cancellationToken );
        }

        #endregion
    }
}
