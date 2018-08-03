#region Namespaces

using Achilles.Entities.Configuration;
using Achilles.Entities.Sqlite.Configuration;
using Entities.Sqlite.Tests.Data;
using System;
using System.IO;
using Xunit;

#endregion

namespace Entities.Sqlite.Tests
{
    public class DbContextServicesTest
    {
        [Fact]
        public void Create_throws_when_invalid_format_connection_string()
        {
            string connectionString = "asdf";

            var ex = Assert.Throws<ArgumentException>( () =>
            {
                var context = new TestDbContext(
                    new DbContextOptionsBuilder()
                        .UseSqlite( connectionString )
                        .Options );

                var result = context.Database.Connection.DbConnection.Database;
            });
            
            Assert.StartsWith( "Format", ex.Message );
        }

        [Fact]
        public void Create_opens_valid_connection()
        {
            string connectionString = "Data Source=test.db";

            using ( var context = TestDbContext.Create( connectionString ) )
            {
                var result = context.Database.Connection.DbConnection.DataSource;

                Assert.EndsWith( "test.db", Path.GetFileName( result ) );
            }
        }

        [Fact]
        public void Create_opens_valid_memory_connection()
        {
            string connectionString = "Data Source=:memory:";

            // FIXME: Why does the generic options fail
            //var options = new DbContextOptionsBuilder<TestDbContext>().UseSqlite( connectionString ).Options;
            var options = new DbContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDbContext( options ) )
            {
                var result = context.Database.Connection.DbConnection.Database;
                Assert.Equal( "main", result );
            }
        }
    }
}
