#region Namespaces

using Achilles.Entities.Configuration;
using Achilles.Entities.Properties;
using Achilles.Entities.Sqlite.Configuration;
using Entities.Sqlite.Tests.Data;
using Microsoft.Data.Sqlite;
using System;
using Xunit;

#endregion

namespace Entities.Sqlite.Tests
{
    public class DbContextOptionsTest
    {
        [Fact]
        public void Options_with_command_timeout()
        {
            var optionsBuilder = new DataContextOptionsBuilder();
            optionsBuilder.UseSqlite( "Database=TestDb", o => o.CommandTimeout( 45 ) );

            var options = optionsBuilder.Options as SqliteOptions;

            Assert.NotNull( options );
            Assert.Equal( 45, options.CommandTimeout );
        }

        [Fact]
        public void Options_throws_with_invalid_command_timeout()
        {
            var commandTimeout = -1;

            var ex = Assert.Throws<ArgumentOutOfRangeException>( () =>
            {
                var optionsBuilder = new DataContextOptionsBuilder();
                optionsBuilder.UseSqlite( "Database=test.db", o => o.CommandTimeout( commandTimeout ) );
            } );

           Assert.StartsWith( nameof( commandTimeout ), ex.ParamName );
        }

        [Fact]
        public void Options_throws_when_null_connection_string()
        {
            string connectionString = null;

            var ex = Assert.Throws<ArgumentException>( () =>
            {
                var optionsBuilder = new DataContextOptionsBuilder<TestDataContext>().UseSqlite( connectionString );

                // var context = new TestDbContext( optionsBuilder.Options );
            } );

            Assert.StartsWith( Resources.ConnectionStringCannotBeNullOrEmpty, ex.Message );
        }

        [Fact]
        public void Options_throws_when_empty_connection_string()
        {
            string connectionString = "";

            var ex = Assert.Throws<ArgumentException>( () =>
            {
                var options = new DataContextOptionsBuilder<TestDataContext>().UseSqlite( connectionString ).Options;

                //new TestDbContext( options );
            } );

            Assert.StartsWith( Resources.ConnectionStringCannotBeNullOrEmpty, ex.Message );
        }

        [Fact]
        public void Options_throws_when_null_connection()
        {
            SqliteConnection connection = null;

            var ex = Assert.Throws<ArgumentNullException>( () =>
            {
                var options = new DataContextOptionsBuilder<TestDataContext>().UseSqlite( connection ).Options;
            } );

            Assert.StartsWith( "Value cannot be null.", ex.Message );
            Assert.StartsWith( nameof(connection), ex.ParamName );
        }
    }
}
