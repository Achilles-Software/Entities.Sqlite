using Achilles.Entities.Storage;
using Entities.Sqlite.Tests.Data;
using Entities.Sqlite.Tests.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Entities.Sqlite.Tests.Relational
{
    public class DatabaseCreationTest
    {
        [Fact]
        public void Exists_returns_true_when_database_exists()
        {
            using ( var testStore = SqliteTest<TestDbContext>.Create( "Empty" ) )
            {
                //var database = testStore.ServiceProvider.GetService<IRelationalDatabaseCreator>();
                //Assert.True( creator.Database.Exists() );
            }
        }
    }
}
