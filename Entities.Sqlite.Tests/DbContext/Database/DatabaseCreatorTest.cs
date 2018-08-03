#region Namespaces

using Achilles.Entities.Configuration;
using Achilles.Entities.Sqlite.Configuration;
using Entities.Sqlite.Tests.Data;
using Xunit;

#endregion

namespace Entities.Sqlite.Tests
{
    public class DatabaseCreatorTest
    {
        [Fact]
        public void DatabaseCreator_CreateIfNotExists_ExistsAndHasTables()
        {
            const string connectionString = "Data Source=:memory:";
            var options = new DbContextOptionsBuilder().UseSqlite( connectionString ).Options;

            using ( var context = new TestDbContext( options ) )
            {
                var result = context.Database.Connection.DbConnection.Database;
                Assert.Equal( "main", result );

                Assert.True( context.Database.Exists() );
                Assert.False( context.Database.HasTables() );

                var createResult = context.Database.Creator.CreateIfNotExists();
                Assert.True( createResult );
                Assert.True( context.Database.Exists() );
                Assert.True( context.Database.HasTables() );
            }
        }
    }
}
