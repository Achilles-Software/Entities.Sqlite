#region Namespaces

using Achilles.Entities.Configuration;
using Achilles.Entities.Relational.Modelling;
using Achilles.Entities.Relational.Statements;
using Achilles.Entities.Sqlite.Modelling;
using Achilles.Entities.Sqlite.Statements;
using Achilles.Entities.Sqlite.Storage;
using Achilles.Entities.Storage;
using Microsoft.Extensions.DependencyInjection;
using System;

#endregion

namespace Achilles.Entities.Sqlite.Configuration
{
    public static class SqliteServiceCollectionExtensions
    {
        public static IServiceCollection AddSqliteServices( this IServiceCollection serviceCollection )
        {
            if ( serviceCollection == null )
            {
                throw new ArgumentNullException( nameof( serviceCollection ) );
            }

            var builder = new RelationalServicesBuilder( serviceCollection )
                .TryAddTransient<IRelationalModelBuilder, SqliteRelationalModelBuilder>()
                .TryAddSingleton<IRelationalConnection, SqliteRelationalConnection>()
                .TryAddSingleton<IRelationalDatabase, SqliteRelationalDatabase>()
                .TryAddSingleton<IRelationalDatabaseCreator, SqliteDatabaseCreator>()
                .TryAddSingleton<IRelationalCommandBuilder, SqliteRelationalCommandBuilder>();


            builder.TryAddCoreServices();

            return serviceCollection;
        }
    }
}
