#region Namespaces

using Achilles.Entities.Configuration;
using Achilles.Entities.Properties;
using System;
using System.Data.Common;

#endregion

namespace Achilles.Entities.Sqlite.Configuration
{
    public static class SqliteDbContextOptionsBuilderExtensions
    {
        //public static DbContextOptionsBuilder<TContext> UseSqlite<TContext>(
        //    this DbContextOptionsBuilder<TContext> optionsBuilder,
        //    string connectionString,
        //    Action<SqliteDbContextOptionsBuilder> sqliteOptionsAction = null )
        //    where TContext : DbContext
        //    => (DbContextOptionsBuilder<TContext>)UseSqlite(
        //        (DbContextOptionsBuilder)optionsBuilder, connectionString, sqliteOptionsAction );

        //public static DbContextOptionsBuilder<TContext> UseSqlite<TContext>(
        //    this DbContextOptionsBuilder<TContext> optionsBuilder,
        //    DbConnection connection,
        //    Action<SqliteDbContextOptionsBuilder> sqliteOptionsAction = null )
        //    where TContext : DbContext
        //    => (DbContextOptionsBuilder<TContext>)UseSqlite(
        //        (DbContextOptionsBuilder)optionsBuilder, connection, sqliteOptionsAction );

        public static DataContextOptionsBuilder UseSqlite(
            this DataContextOptionsBuilder optionsBuilder,
            string connectionString,
            Action<SqliteDbContextOptionsBuilder> sqliteOptionsAction = null )
        {
            if ( optionsBuilder == null )
            {
                throw new ArgumentNullException( nameof( optionsBuilder ) );
            }

            if ( string.IsNullOrEmpty( connectionString ) )
            {
                throw new ArgumentException( Resources.ConnectionStringCannotBeNullOrEmpty, nameof( connectionString ) );
            }

            var options = new SqliteOptions().WithConnectionString( connectionString );
            optionsBuilder.Options = options;

            sqliteOptionsAction?.Invoke( new SqliteDbContextOptionsBuilder( optionsBuilder ) );

            return optionsBuilder;
        }

        public static DataContextOptionsBuilder UseSqlite(
            this DataContextOptionsBuilder optionsBuilder,
            DbConnection connection,
            Action<SqliteDbContextOptionsBuilder> sqliteOptionsAction = null )
        {
            if ( optionsBuilder == null )
            {
                throw new ArgumentNullException( nameof( optionsBuilder ) );
            }

            if ( connection == null )
            {
                throw new ArgumentNullException( nameof( connection ) );
            }

            var options = new SqliteOptions().WithConnection( connection );
            optionsBuilder.Options = options;

            sqliteOptionsAction?.Invoke( new SqliteDbContextOptionsBuilder( optionsBuilder ) );

            return optionsBuilder;
        }       
    }
}
