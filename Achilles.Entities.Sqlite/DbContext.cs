using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Achilles.Entities
{
    public class DbContext : IDbContext, IDisposable
    {
        //public IDbConnection ConnectionFactory { get; private set; }

        public IDbConnection Connection { get; private set; }

        public DbContext( string nameOrConnectionString ) //, IDbConnectionFactory connectionFactory )
        {
            //ConnectionFactory = connectionFactory;
            //Connection = ConnectionFactory.CreateIfNotExists( nameOrConnectionString );
        }

        //protected internal virtual void OnConfiguring( DbContextOptionsBuilder optionsBuilder )
        //{
        //}

        //public static IDbConnectionFactory CreateDefaultConnectionFactory()
        //{
        //    return DbConnectionFactory.Get();
        //}

        public IEnumerable<T> SqlQuery<T>( string sql ) where T : class, new()
        {
            return QueryInternal<T>( sql, null );
        }

        public IEnumerable<T> SqlQuery<T>( string sql, object parameters ) where T : class, new()
        {
            return QueryInternal<T>( sql, parameters );
        }

        internal IEnumerable<T> QueryInternal<T>( string sql, object parameters ) where T : class
        {
            // TODO: SOC/SRP violated. Wrap this logic with a different class.
            Action<IDbCommand, object> paramReader = ( command, obj ) =>
            {
                var properties = obj.GetType()
                    .GetProperties()
                    .Select( property => new { Name = property.Name, Value = property.GetValue( obj, null ) } )
                    .ToList();

                foreach ( var propertyMap in properties )
                {
                    IDbDataParameter dbparam = command.CreateParameter();
                    dbparam.ParameterName = propertyMap.Name;
                    dbparam.Value = propertyMap.Value;
                    command.Parameters.Add( dbparam );
                }
            };

            using ( IDbCommand command = SetupStoredCommand( null, sql, parameters != null ? paramReader : null, parameters, null ) )
            {
                //Connection.OpenIfNot();

                using ( IDataReader reader = command.ExecuteReader() )
                {
                    // Materializer<T> mapper = new Materializer<T>( reader );
                    while ( reader.Read() )
                    {
                        yield return null;// (T)mapper.Materialize( reader );
                    }
                }
            }
        }

        private IDbCommand SetupStoredCommand( IDbTransaction transaction, string sql, Action<IDbCommand, object> paramReader, object obj, int? commandTimeout )
        {
            return SetupCommand( transaction, sql, paramReader, obj, commandTimeout, CommandType.StoredProcedure );
        }

        private IDbCommand SetupCommand( IDbTransaction transaction, string sql, Action<IDbCommand, object> paramReader, object obj, int? commandTimeout, CommandType? commandType )
        {
            IDbCommand command = Connection.CreateCommand();
            if ( transaction != null )
            {
                command.Transaction = transaction;
            }
            if ( commandTimeout.HasValue )
            {
                command.CommandTimeout = commandTimeout.Value;
            }
            if ( commandType.HasValue )
            {
                command.CommandType = commandType.Value;
            }
            command.CommandText = sql;
            if ( paramReader != null )
            {
                paramReader( command, obj );
            }

            return command;
        }

        public void Dispose()
        {
            Dispose( true );
            GC.SuppressFinalize( this );
        }

        protected void Dispose( bool disposing )
        {
            if ( disposing )
            {
                using ( Connection )
                {
                    Connection.Close();
                }
            }
        }
    }
}
