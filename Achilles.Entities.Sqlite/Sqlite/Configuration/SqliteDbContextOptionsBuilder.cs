#region Namespaces

using Achilles.Entities.Configuration;
using Achilles.Entities.Relational.Configuration;
using Microsoft.Extensions.DependencyInjection;

#endregion

namespace Achilles.Entities.Sqlite.Configuration
{
    public class SqliteDbContextOptionsBuilder : RelationalDbContextOptionsBuilder<SqliteDbContextOptionsBuilder, SqliteOptions>
    {
        public SqliteDbContextOptionsBuilder( DataContextOptionsBuilder optionsBuilder )
            : base( optionsBuilder )
        {
        }

        public virtual SqliteDbContextOptionsBuilder SuppressForeignKeyEnforcement( bool suppressForeignKeyEnforcement = true )
           => WithOption( e => e.WithEnforceForeignKeys( !suppressForeignKeyEnforcement ) );

        protected override void AddServices( IServiceCollection services )
        {
            services.AddSqliteServices();
        }
    }
}
