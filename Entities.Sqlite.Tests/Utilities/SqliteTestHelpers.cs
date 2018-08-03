#region Namespaces

using Achilles.Entities.Configuration;
using Achilles.Entities.Sqlite.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace Entities.Sqlite.Tests.Utilities
{
    public class SqliteTestHelpers : TestHelpers
    {
        protected SqliteTestHelpers()
        {
        }

        public static SqliteTestHelpers Instance { get; } = new SqliteTestHelpers();

        public override IServiceCollection AddProviderServices( IServiceCollection services )
            => services.AddSqliteServices();

        protected override void UseProviderOptions( DbContextOptionsBuilder optionsBuilder )
            => optionsBuilder.UseSqlite( new SqliteConnection( "Data Source=:memory:" ) );
    }
}
