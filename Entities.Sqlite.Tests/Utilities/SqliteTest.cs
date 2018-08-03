#region Namespaces

using Achilles.Entities;
using Achilles.Entities.Sqlite.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;

#endregion

namespace Entities.Sqlite.Tests.Utilities
{
    public class SqliteTest<TContext> : IDisposable where TContext : DbContext
    {
        public const int CommandTimeout = 30;
        public string Name { get; }

        public IServiceProvider ServiceProvider { get; }
        protected virtual Type ContextType { get; } = typeof( TContext );
        public DbConnection Connection { get; }
        public virtual string ConnectionString { get;  }
        
        public SqliteTest( string name, bool sharedCache )
        {
            Name = name;

            ConnectionString = new SqliteConnectionStringBuilder
            {
                DataSource = Name + ".db",
                Cache = sharedCache ? SqliteCacheMode.Shared : SqliteCacheMode.Private

            }.ToString();

            var services = AddSqliteServices( new ServiceCollection() );

            Connection = new SqliteConnection( ConnectionString );

            services = services.AddDbContext<TContext>( options =>
                options.UseSqlite( Connection ),
                ServiceLifetime.Transient, ServiceLifetime.Singleton );

            ServiceProvider = services.BuildServiceProvider( validateScopes: true );
        }

        /// <summary>
        /// Creates a SqliteTest instance.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sharedCache"></param>
        /// <returns></returns>
        public static SqliteTest<TContext> Create( string name, bool sharedCache = true )
            => new SqliteTest<TContext>( name, sharedCache: sharedCache );

        public ConnectionState ConnectionState => Connection.State;

        public void CloseConnection() => Connection.Close();

        public virtual Task OpenConnectionAsync() => Connection.OpenAsync();

        public DbTransaction BeginTransaction() => Connection.BeginTransaction();

        public void Clean( DbContext context )
        { 
            //=> context.Database.EnsureClean();
        }

        public IServiceCollection AddSqliteServices( IServiceCollection serviceCollection )
            => serviceCollection.AddSqliteServices();

        public void OpenConnection()
        {
            Connection.Open();

            using (var command = Connection.CreateCommand())
            {
                command.CommandText = "PRAGMA foreign_keys=ON;";
                command.ExecuteNonQuery();
            }
        }

        public int ExecuteNonQuery(string sql, params object[] parameters)
        {
            using (var command = CreateCommand(sql, parameters))
            {
                return command.ExecuteNonQuery();
            }
        }

        private DbCommand CreateCommand(string commandText, object[] parameters)
        {
            var command = (SqliteCommand)Connection.CreateCommand();

            command.CommandText = commandText;
            command.CommandTimeout = CommandTimeout;

            for (var i = 0; i < parameters.Length; i++)
            {
                command.Parameters.AddWithValue("@p" + i, parameters[i]);
            }

            return command;
        }

        public void Dispose()
        {
            Connection?.Dispose();
        }
    }
}
